
Imports System
Imports System.Type
Imports System.Activator
Imports System.Runtime.InteropServices
Imports Inventor

Public Class Form1
    Public Shared _invApp As Inventor.Application
    Dim _Doc As Inventor.PartDocument
    Dim _started As Boolean = False
    Dim _SplitDir As Integer
    Dim _CompDef As PartComponentDefinition
    Dim _SecondarySlicesNumber As Integer
    Dim _initialComp As Integer
    Dim _PullDir As Integer = 1
    Dim _PrimExtra As Integer = 0
    Dim _SecondaryExtra As Integer = 0
    Dim _BodyIndex As Integer = 0

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Try
            _invApp = Marshal.GetActiveObject("Inventor.Application")
        Catch ex As Exception
            Try
                Dim invAppType As Type = GetTypeFromProgID("Inventor.Application")
                _invApp = CreateInstance(invAppType)
                _invApp.Visible = True
                _started = True
                MsgBox("Inventor Started")

            Catch ex2 As Exception
                MsgBox(ex2.ToString())
                MsgBox("unable to start Inventor")
            End Try
        End Try

        If _invApp.Documents.Count = 0 Then
            MsgBox("Need to open a Part Document")
            Return
        End If

        If _invApp.ActiveDocument.DocumentType <> DocumentTypeEnum.kPartDocumentObject Then
            MsgBox("Need to have a Part Document active")
            Return
        End If

        _Doc = _invApp.ActiveDocument
        _CompDef = _Doc.ComponentDefinition
        _initialComp = _CompDef.SurfaceBodies.Count
        If Puzzletest() Then
            MsgBox("remove previous puzzle first")
        End If

    End Sub

    '************ Creating Puzzle ***********

    Function Puzzletest() As Boolean

        Dim PreviousPuzzle As Boolean = False
        Dim Testplane As WorkPlane
        For Each Testplane In _CompDef.WorkPlanes
            If (Testplane.Name = "Start Puzzle") Then
                PreviousPuzzle = True
                Exit For
            End If
        Next
        Return PreviousPuzzle
    End Function

    Private Sub GetBodyButton_Click(sender As Object, e As EventArgs) Handles GetBodyButton.Click
        Dim body As SurfaceBody = GetBody()
        Dim MainDir As KeyValuePair(Of Integer, Double) = GetBoundingBoxLength(body)
        GenerateSlices(MainDir, body)
        makebodiesvisible()
        CreateIntersections()
    End Sub

    Function GetBody() As SurfaceBody
        ' Have the bodies selected. 
        Dim baseBody As SurfaceBody
        baseBody = _invApp.CommandManager.Pick(SelectionFilterEnum.kPartBodyFilter, "Select the base body")
        Return baseBody
    End Function

    Function GetBoundingBoxLength(basebody As SurfaceBody) As KeyValuePair(Of Integer, Double)
        Dim BoundingBox As Box
        Dim MainDir As Integer
        BoundingBox = basebody.RangeBox

        Dim Xlength As Double
        Xlength = BoundingBox.MaxPoint.X - BoundingBox.MinPoint.X

        Dim Ylength As Double
        Ylength = BoundingBox.MaxPoint.Y - BoundingBox.MinPoint.Y

        Dim Zlength As Double
        Zlength = BoundingBox.MaxPoint.Z - BoundingBox.MinPoint.Z

        Dim Length As Double

        If Zdir.Checked Then
            _SplitDir = 3
            If Xlength >= Ylength Then
                Length = Xlength
                MainDir = 1
            Else
                Length = Ylength
                MainDir = 2
            End If
        ElseIf Xdir.Checked Then
            _SplitDir = 1
            If Ylength >= Zlength Then
                Length = Ylength
                MainDir = 2
            Else
                Length = Zlength
                MainDir = 3
            End If
        Else
            _SplitDir = 2
            If Ydir.Checked Then
                Length = Xlength
                MainDir = 1
            Else
                Length = Zlength
                MainDir = 3
            End If
        End If

        Return New KeyValuePair(Of Integer, Double)(MainDir, Length)

    End Function

    Sub GenerateSlices(MainDir As KeyValuePair(Of Integer, Double), baseBody As SurfaceBody)

        Dim basePlane As WorkPlane
        Dim secondaryPlane As WorkPlane
        Dim width As Double
        Dim secondaryOffset As Double
        Dim Spacing As Double

        Spacing = MainDir.Value / NumberOfSlices.Value

        _BodyIndex = _initialComp + 1

        'Dim boundingboxXmin As WorkPlane
        'Dim boundingboxXmax As WorkPlane
        'Dim boundingboxYmin As WorkPlane
        'Dim boundingboxYmax As WorkPlane
        'boundingboxXmin = _CompDef.WorkPlanes.AddByPlaneAndOffset(_CompDef.WorkPlanes.Item(1), baseBody.RangeBox.MinPoint.X)
        'boundingboxXmax = _CompDef.WorkPlanes.AddByPlaneAndOffset(_CompDef.WorkPlanes.Item(1), baseBody.RangeBox.MaxPoint.X)
        'boundingboxYmin = _CompDef.WorkPlanes.AddByPlaneAndOffset(_CompDef.WorkPlanes.Item(2), baseBody.RangeBox.MinPoint.Y)
        'boundingboxYmax = _CompDef.WorkPlanes.AddByPlaneAndOffset(_CompDef.WorkPlanes.Item(2), baseBody.RangeBox.MaxPoint.Y)

        Select Case MainDir.Key
            Case 1
                basePlane = _CompDef.WorkPlanes.AddByPlaneAndOffset(_CompDef.WorkPlanes.Item(1), baseBody.RangeBox.MinPoint.X + Spacing / 2)
                If Zdir.Checked Then
                    width = baseBody.RangeBox.MaxPoint.Y - baseBody.RangeBox.MinPoint.Y
                    _SecondarySlicesNumber = Math.Floor(width / Spacing)
                    secondaryOffset = (width - (_SecondarySlicesNumber - 1) * Spacing) / 2
                    secondaryPlane = _CompDef.WorkPlanes.AddByPlaneAndOffset(_CompDef.WorkPlanes.Item(2), baseBody.RangeBox.MinPoint.Y + secondaryOffset)

                Else
                    width = baseBody.RangeBox.MaxPoint.Z - baseBody.RangeBox.MinPoint.Z
                    _SecondarySlicesNumber = Math.Floor(width / Spacing)
                    secondaryOffset = (width - (_SecondarySlicesNumber - 1) * Spacing) / 2
                    secondaryPlane = _CompDef.WorkPlanes.AddByPlaneAndOffset(_CompDef.WorkPlanes.Item(3), baseBody.RangeBox.MinPoint.Z + secondaryOffset)
                End If

            Case 2
                basePlane = _CompDef.WorkPlanes.AddByPlaneAndOffset(_CompDef.WorkPlanes.Item(2), baseBody.RangeBox.MinPoint.Y + Spacing / 2)
                If Zdir.Checked Then
                    width = baseBody.RangeBox.MaxPoint.X - baseBody.RangeBox.MinPoint.X
                    _SecondarySlicesNumber = Math.Floor(width / Spacing)
                    secondaryOffset = (width - (_SecondarySlicesNumber - 1) * Spacing) / 2
                    secondaryPlane = _CompDef.WorkPlanes.AddByPlaneAndOffset(_CompDef.WorkPlanes.Item(1), baseBody.RangeBox.MinPoint.X + secondaryOffset)

                Else
                    width = baseBody.RangeBox.MaxPoint.Z - baseBody.RangeBox.MinPoint.Z
                    _SecondarySlicesNumber = Math.Floor(width / Spacing)
                    secondaryOffset = (width - (_SecondarySlicesNumber - 1) * Spacing) / 2
                    secondaryPlane = _CompDef.WorkPlanes.AddByPlaneAndOffset(_CompDef.WorkPlanes.Item(3), baseBody.RangeBox.MinPoint.Z + secondaryOffset)

                End If
            Case Else
                basePlane = _CompDef.WorkPlanes.AddByPlaneAndOffset(_CompDef.WorkPlanes.Item(3), baseBody.RangeBox.MinPoint.Z + Spacing / 2)
                If Ydir.Checked Then
                    width = baseBody.RangeBox.MaxPoint.X - baseBody.RangeBox.MinPoint.X
                    _SecondarySlicesNumber = Math.Floor(width / Spacing)
                    secondaryOffset = (width - (_SecondarySlicesNumber - 1) * Spacing) / 2
                    secondaryPlane = _CompDef.WorkPlanes.AddByPlaneAndOffset(_CompDef.WorkPlanes.Item(1), baseBody.RangeBox.MinPoint.X + secondaryOffset)

                Else
                    width = baseBody.RangeBox.MaxPoint.Y - baseBody.RangeBox.MinPoint.Y
                    _SecondarySlicesNumber = Math.Floor(width / Spacing)
                    secondaryOffset = (width - (_SecondarySlicesNumber - 1) * Spacing) / 2
                    secondaryPlane = _CompDef.WorkPlanes.AddByPlaneAndOffset(_CompDef.WorkPlanes.Item(2), baseBody.RangeBox.MinPoint.Y + secondaryOffset)

                End If
        End Select

        basePlane.Name = "Start Puzzle"
        basePlane.Visible = False
        secondaryPlane.Visible = False

        Dim SliceIndex As Integer
        Dim SlicePlane As WorkPlane
        Dim Ranking As String

        For SliceIndex = 1 To NumberOfSlices.Value
            Ranking = "P"
            SlicePlane = _CompDef.WorkPlanes.AddByPlaneAndOffset(basePlane, (SliceIndex - 1) * Spacing)
            SlicePlane.Visible = False
            CreateSlice(SlicePlane, baseBody, Ranking, SliceIndex)
        Next

        For SliceIndex = 1 To _SecondarySlicesNumber
            Ranking = "S"
            SlicePlane = _CompDef.WorkPlanes.AddByPlaneAndOffset(secondaryPlane, (SliceIndex - 1) * Spacing)
            SlicePlane.Visible = False
            CreateSlice(SlicePlane, baseBody, Ranking, SliceIndex)
        Next
    End Sub

    Sub CreateSlice(SlicePlane As WorkPlane, Body As SurfaceBody, Ranking As String, SliceIndex As Integer)
        Dim contoursketch As PlanarSketch = _CompDef.Sketches.Add(SlicePlane)
        Dim ExtrudeThickness = CDbl(SliceThickness.Value)
        contoursketch.ProjectedCuts.Add()

        Dim exturdeprofiletest As Profile
        exturdeprofiletest = contoursketch.Profiles.AddForSolid(True)
        Debug.Print(exturdeprofiletest.Count)

        If exturdeprofiletest.Count = 1 Then
            Dim extrudetest As ExtrudeDefinition
            extrudetest = _CompDef.Features.ExtrudeFeatures.CreateExtrudeDefinition(exturdeprofiletest, PartFeatureOperationEnum.kNewBodyOperation)
            Call extrudetest.SetDistanceExtent(ExtrudeThickness, PartFeatureExtentDirectionEnum.kSymmetricExtentDirection)
            _CompDef.Features.ExtrudeFeatures.Add(extrudetest)
            _CompDef.SurfaceBodies.Item(_BodyIndex).Name = Ranking & (SliceIndex)
            _CompDef.SurfaceBodies.Item(_BodyIndex).Visible = False
            _BodyIndex = _BodyIndex + 1
        Else
            Dim i As Integer
            For i = 1 To exturdeprofiletest.Count
                Dim newprof As Profile
                newprof = contoursketch.Profiles.AddForSolid(False)

                Dim j As Integer
                Dim deleteIndex As Integer = 1
                For j = 1 To exturdeprofiletest.Count - 1
                    If i = j Then
                        deleteIndex = 2
                    End If
                    newprof.Item(deleteIndex).Delete()
                Next
                Dim extrudetest As ExtrudeDefinition
                extrudetest = _CompDef.Features.ExtrudeFeatures.CreateExtrudeDefinition(newprof, PartFeatureOperationEnum.kNewBodyOperation)
                Call extrudetest.SetDistanceExtent(ExtrudeThickness, PartFeatureExtentDirectionEnum.kSymmetricExtentDirection)
                _CompDef.Features.ExtrudeFeatures.Add(extrudetest)
                _CompDef.SurfaceBodies.Item(_BodyIndex).Name = Ranking & (SliceIndex) & "-" & i
                _CompDef.SurfaceBodies.Item(_BodyIndex).Visible = False
                _BodyIndex = _BodyIndex + 1
            Next
        End If

        If Ranking = "P" Then
            _PrimExtra = exturdeprofiletest.Count - 1
        Else
            _SecondaryExtra = exturdeprofiletest.Count - 1
        End If

    End Sub

    Sub makebodiesvisible()
        For Each body As SurfaceBody In _CompDef.SurfaceBodies
            body.Visible = True
        Next

        Dim previndex As Integer
        For previndex = 1 To _initialComp
            _CompDef.SurfaceBodies.Item(previndex).Visible = False
        Next

        'Colour first Prim Body
        Dim FirstPrim As SurfaceBody = _CompDef.SurfaceBodies.Item(_initialComp + 1)
        Dim oFace As Face
        For Each oFace In FirstPrim.Faces
            ' Set the render style to be "As Feature". 
            Call oFace.SetRenderStyle(StyleSourceTypeEnum.kOverrideRenderStyle, _Doc.RenderStyles.Item("Green"))
        Next

        'Colour first Secondary Body
        Dim FirstSecondary As SurfaceBody = _CompDef.SurfaceBodies.Item(_initialComp + NumberOfSlices.Value + 1)
        For Each oFace In FirstSecondary.Faces
            ' Set the render style to be "As Feature". 
            Call oFace.SetRenderStyle(StyleSourceTypeEnum.kOverrideRenderStyle, _Doc.RenderStyles.Item("LED - Red On"))
        Next
    End Sub

    Sub CreateIntersections()
        Dim PrimIndex As Integer
        Dim SecIndex As Integer
        Dim BaseBody As SurfaceBody
        Dim ToolBody As SurfaceBody
        Dim progressval As Integer = 0
        Dim progressprocent As Double

        My.Forms.ProgressForm.Show()
        My.Forms.ProgressForm.TopMost = True

        For PrimIndex = 1 To NumberOfSlices.Value
            BaseBody = _CompDef.SurfaceBodies.Item(PrimIndex + _initialComp)
            For SecIndex = 1 To _SecondarySlicesNumber
                ToolBody = _CompDef.SurfaceBodies.Item(SecIndex + NumberOfSlices.Value + _initialComp)
                CreateIntersection(BaseBody, ToolBody)
                progressval = (PrimIndex - 1) * _SecondarySlicesNumber + SecIndex

                progressprocent = progressval / (NumberOfSlices.Value * _SecondarySlicesNumber)

                My.Forms.ProgressForm.ProgressBar1.Value = CInt(progressprocent * My.Forms.ProgressForm.ProgressBar1.Maximum)
                Debug.Print(progressprocent)
            Next

        Next
        My.Forms.ProgressForm.Close()
    End Sub

    Sub CreateIntersection(BaseBody As SurfaceBody, ToolBody As SurfaceBody)

        Dim TransBody As TransientBRep = _invApp.TransientBRep
        Dim transGeo As TransientGeometry = _invApp.TransientGeometry
        Dim objs As ObjectCollection = _invApp.TransientObjects.CreateObjectCollection

        'Create duplicate bodies
        Dim transBase As SurfaceBody = TransBody.Copy(BaseBody)
        Dim transTool As SurfaceBody = TransBody.Copy(ToolBody)

        Try
            'Create boolean
            Call TransBody.DoBoolean(transBase, transTool, BooleanTypeEnum.kBooleanTypeIntersect)
            'Create new solids
            Dim transdef As NonParametricBaseFeatureDefinition
            transdef = _CompDef.Features.NonParametricBaseFeatures.CreateDefinition()
            Call objs.Add(transBase)
            transdef.BRepEntities = objs
            transdef.OutputType = BaseFeatureOutputTypeEnum.kSolidOutputType

            _CompDef.Features.NonParametricBaseFeatures.AddByDefinition(transdef)
        Catch ex As Exception
            Exit Sub
        End Try

        'Get slice plane coordinates
        Dim primtext As String
        Dim primnumber As Integer
        Dim separator As Integer
        Dim stringleng As Integer

        primtext = BaseBody.Name.Remove(0, 1)
        If IsNumeric(primtext) Then
            primnumber = CInt(primtext)
        Else
            separator = primtext.IndexOf("-")
            stringleng = primtext.Length
            primtext = primtext.Remove(separator, (stringleng - separator))
            primnumber = CInt(primtext)
        End If
        Debug.Print(primnumber)

        Dim secondtext As String
        Dim secondnumber As Integer

        secondtext = ToolBody.Name.Remove(0, 1)
        If IsNumeric(secondtext) Then
            secondnumber = CInt(secondtext)
        Else
            separator = primtext.IndexOf("-")
            stringleng = secondtext.Length
            secondtext = secondtext.Remove(separator, (stringleng - separator))
            secondnumber = CInt(secondtext)
        End If


        'Create plane
        Dim LatestBody As SurfaceBody = _CompDef.SurfaceBodies.Item(_CompDef.SurfaceBodies.Count)
        Dim splitDist As Double
        Dim WorkPlns As WorkPlanes = _CompDef.WorkPlanes
        Dim splitplane As WorkPlane
        Dim Point1 As Point2d = _invApp.TransientGeometry.CreatePoint2d
        Dim Point2 As Point2d = _invApp.TransientGeometry.CreatePoint2d

        Select Case _SplitDir
            Case 1
                splitDist = (LatestBody.RangeBox.MaxPoint.X - LatestBody.RangeBox.MinPoint.X) / 2 + LatestBody.RangeBox.MinPoint.X
                splitplane = WorkPlns.AddByPlaneAndOffset(_CompDef.WorkPlanes.Item(1), splitDist)
                Point1.X = LatestBody.RangeBox.MinPoint.Y
                Point1.Y = LatestBody.RangeBox.MinPoint.Z
                Point2.X = LatestBody.RangeBox.MaxPoint.Y
                Point2.Y = LatestBody.RangeBox.MaxPoint.Z
            Case 2
                splitDist = (LatestBody.RangeBox.MaxPoint.Y - LatestBody.RangeBox.MinPoint.Y) / 2 + LatestBody.RangeBox.MinPoint.Y
                splitplane = WorkPlns.AddByPlaneAndOffset(_CompDef.WorkPlanes.Item(2), splitDist)
                Call splitplane.FlipNormal()
                Point1.X = LatestBody.RangeBox.MinPoint.X
                Point1.Y = LatestBody.RangeBox.MinPoint.Z
                Point2.X = LatestBody.RangeBox.MaxPoint.X
                Point2.Y = LatestBody.RangeBox.MaxPoint.Z

            Case Else
                splitDist = (LatestBody.RangeBox.MaxPoint.Z - LatestBody.RangeBox.MinPoint.Z) / 2 + LatestBody.RangeBox.MinPoint.Z
                splitplane = WorkPlns.AddByPlaneAndOffset(_CompDef.WorkPlanes.Item(3), splitDist)
                Point1.X = LatestBody.RangeBox.MinPoint.X
                Point1.Y = LatestBody.RangeBox.MinPoint.Y
                point2.X = LatestBody.RangeBox.MaxPoint.X
                Point2.Y = LatestBody.RangeBox.MaxPoint.Y
        End Select

        splitplane.Visible = False

        'Delete latest body feature
        _CompDef.Features.NonParametricBaseFeatures.Item(1).Delete()

        'Create extrude
        Dim UpperSketch As PlanarSketch = _CompDef.Sketches.Add(splitplane)
        UpperSketch.SketchLines.AddAsTwoPointRectangle(Point1, Point2)
        Dim UpperProfile As Profile = UpperSketch.Profiles.AddForSolid()
        Dim UpperExtDef As ExtrudeDefinition = _CompDef.Features.ExtrudeFeatures.CreateExtrudeDefinition(UpperProfile, PartFeatureOperationEnum.kCutOperation)

        Dim Uppercoll As ObjectCollection = _invApp.TransientObjects.CreateObjectCollection
        Call Uppercoll.Add(BaseBody)
        UpperExtDef.AffectedBodies = Uppercoll
        Call UpperExtDef.SetThroughAllExtent(PartFeatureExtentDirectionEnum.kPositiveExtentDirection)
        _CompDef.Features.ExtrudeFeatures.Add(UpperExtDef)

        Dim LowerSketch As PlanarSketch = _CompDef.Sketches.Add(splitplane)
        LowerSketch.SketchLines.AddAsTwoPointRectangle(Point1, Point2)
        Dim LowerProfile As Profile = LowerSketch.Profiles.AddForSolid()
        Dim LowerExtDef As ExtrudeDefinition = _CompDef.Features.ExtrudeFeatures.CreateExtrudeDefinition(LowerProfile, PartFeatureOperationEnum.kCutOperation)

        Dim Lowercoll As ObjectCollection = _invApp.TransientObjects.CreateObjectCollection
        Call Lowercoll.Add(ToolBody)
        LowerExtDef.AffectedBodies = Lowercoll
        Call LowerExtDef.SetThroughAllExtent(PartFeatureExtentDirectionEnum.kNegativeExtentDirection)
        _CompDef.Features.ExtrudeFeatures.Add(LowerExtDef)

    End Sub

    '************* Nesting ************

    Private Sub NestButton_Click(sender As Object, e As EventArgs) Handles NestButton.Click
        FlattenBodies()
        NestBodies()
    End Sub

    Private Sub FlattenBodies()

    End Sub

    Private Sub NestBodies()

    End Sub

    '************* Radio toggles *********

    Private Sub Xdir_CheckedChanged(sender As Object, e As EventArgs)
        If Xdir.Checked Then
            Ydir.Checked = False
            Zdir.Checked = False
            _PullDir = 1
        End If
    End Sub

    Private Sub Ydir_CheckedChanged(sender As Object, e As EventArgs)
        If Ydir.Checked Then
            Xdir.Checked = False
            Zdir.Checked = False
            _PullDir = 2
        End If
    End Sub

    Private Sub Zdir_CheckedChanged(sender As Object, e As EventArgs)
        If Zdir.Checked Then
            Xdir.Checked = False
            Ydir.Checked = False
            _PullDir = 3
        End If
    End Sub


End Class

