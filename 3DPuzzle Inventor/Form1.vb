
Imports System
Imports System.Type
Imports System.Activator
Imports System.Runtime.InteropServices
Imports Inventor
Imports System.IO


Public Class Form1
    Public Shared _invApp As Inventor.Application
    Dim _Doc As Inventor.PartDocument
    Dim _started As Boolean = False
    Dim _SplitDir As Integer
    Dim _CompDef As PartComponentDefinition
    Dim _SecondarySlicesNumber As Integer
    Dim _initialComp As Integer
    Dim _PullDir As Integer = 1
    Dim _BodyIndex As Integer = 0
    Dim _PrimExtra As Integer = 0
    Dim _SecondaryExtra As Integer = 0
    Dim _Spacing As Double
    Dim _BaseBodyCenter As Double
    Dim _secondaryPlane As WorkPlane
    Dim _Filelocation As String
    Dim _PrimPlates As New List(Of Plate)
    Dim _SeconPlates As New List(Of Plate)
    Dim _DebugMode As Boolean = False
    Dim _Colormode As Boolean = True

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

    End Sub

    '************ Start Puzzle ***********

    Private Sub GetBodyButton_Click(sender As Object, e As EventArgs) Handles GetBodyButton.Click

        Dim body As SurfaceBody = GetBody()
        Dim MainDir As KeyValuePair(Of Integer, Double) = GetBoundingBoxLength(body)

        If Puzzletest() Then
            Dim msg1res As DialogResult
            msg1res = MsgBox("A puzzle was already created, would you like to delete it?", MsgBoxStyle.YesNo, "Puzzletest") '& MsgBoxStyle.SystemModal, "On Top"
            If msg1res = DialogResult.Yes Then
                Exit Sub
                'deletefeatures()
            Else
                Exit Sub
            End If
        End If

        _initialComp = _CompDef.SurfaceBodies.Count
        If _DebugMode Then
            boundingboxcheck(body)
        End If
        GenerateSlices(MainDir, body)
        makebodiesvisible()
        CreateIntersections()
        DXFExport()

        Dim FileMsg As DialogResult
        FileMsg = MsgBox("All contours have been exported and can be found here:" & vbNewLine & _Filelocation & vbNewLine & "Click YES to go to the file location", MsgBoxStyle.YesNo)

        If FileMsg = DialogResult.Yes Then
            Process.Start(_Filelocation)
        End If

        My.Forms.Form1.Close()
    End Sub

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

    Sub deletefeatures()
        'Does not work yet... :-(
        Dim oBrowserNode As BrowserNode
        Dim oBrowserPanes As BrowserPanes = _Doc.BrowserPanes
        Dim oBrowserPane As BrowserPane = (From _pan As BrowserPane In oBrowserPanes Where _pan.TreeBrowser Select _pan).FirstOrDefault
        Dim dodelete As Boolean = True
        Dim oCollection As ObjectCollection = _invApp.TransientObjects.CreateObjectCollection

        If dodelete Then
            oBrowserNode = oBrowserPane.TopNode.BrowserNodes.Item(oBrowserPane.TopNode.BrowserNodes.Count - 1)
            If oBrowserNode.BrowserNodeDefinition.Label = "START PUZZLE" Then
                dodelete = False
            End If
            oBrowserNode.Delete()
        End If

    End Sub

    Sub DeleteEOP()

        Dim BrsPanes As BrowserPanes = _invApp.ActiveDocument.BrowserPanes
        Dim BrsPan As Inventor.BrowserPane = (From _Pan As BrowserPane In BrsPanes Where _Pan.TreeBrowser Select _Pan).FirstOrDefault

        Dim DoDelete As Boolean = False
        Dim ObjCollection As ObjectCollection = _invApp.TransientObjects.CreateObjectCollection()

        'Dim PuzzleStart As Integer

        'For Each _node As BrowserNode In BrsPan.TopNode.BrowserNodes
        '    If _node.BrowserNodeDefinition.Label = "START PUZZLE" Then
        '        _node.
        '    End If
        'Next

        _CompDef.en

        For Each _node As BrowserNode In BrsPan.TopNode.BrowserNodes
            If DoDelete Then
                Dim obj = _node.NativeObject
                ObjCollection.Add(obj)
            End If
            Dim def = _node.BrowserNodeDefinition
            Dim x = def.Label
            If x.ToUpper().Contains("END OF PART") Then
                DoDelete = True
            End If
        Next

        _CompDef.DeleteObjects(ObjCollection)
    End Sub

    Function GetBody() As SurfaceBody
        ' Have the bodies selected. 
        Dim baseBody As SurfaceBody
        baseBody = _invApp.CommandManager.Pick(SelectionFilterEnum.kPartBodyFilter, "Select the base body")
        Return baseBody
    End Function

    '*********** Generate Slices ************

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
            _BaseBodyCenter = Zlength / 2 + BoundingBox.MinPoint.Z
            If Xlength >= Ylength Then
                Length = Xlength
                MainDir = 1
            Else
                Length = Ylength
                MainDir = 2
            End If
        ElseIf Xdir.Checked Then
            _SplitDir = 1
            _BaseBodyCenter = Xlength / 2 + BoundingBox.MinPoint.X
            If Ylength >= Zlength Then
                Length = Ylength
                MainDir = 2
            Else
                Length = Zlength
                MainDir = 3
            End If
        Else
            _SplitDir = 2
            _BaseBodyCenter = Ylength / 2 + BoundingBox.MinPoint.Y

            If Xlength >= Zlength Then
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
        Dim mainposition As Double
        Dim secondarypostion As Double

        Dim basePlane As WorkPlane

        _Spacing = MainDir.Value / NumberOfSlices.Value

        _BodyIndex = _initialComp + 1

        Select Case MainDir.Key
            Case 1
                mainposition = baseBody.RangeBox.MinPoint.X + _Spacing / 2
                basePlane = _CompDef.WorkPlanes.AddByPlaneAndOffset(_CompDef.WorkPlanes.Item(1), mainposition)
                If Zdir.Checked Then
                    secondarypostion = SecondaryStart(baseBody, 2)
                Else
                    secondarypostion = SecondaryStart(baseBody, 3)
                End If

            Case 2
                mainposition = baseBody.RangeBox.MinPoint.Y + _Spacing / 2
                basePlane = _CompDef.WorkPlanes.AddByPlaneAndOffset(_CompDef.WorkPlanes.Item(2), mainposition)
                If Zdir.Checked Then
                    secondarypostion = SecondaryStart(baseBody, 1)
                Else
                    secondarypostion = SecondaryStart(baseBody, 3)
                End If
            Case Else
                mainposition = baseBody.RangeBox.MinPoint.Z + _Spacing / 2
                basePlane = _CompDef.WorkPlanes.AddByPlaneAndOffset(_CompDef.WorkPlanes.Item(3), mainposition)
                If Ydir.Checked Then
                    secondarypostion = SecondaryStart(baseBody, 1)
                Else
                    secondarypostion = SecondaryStart(baseBody, 2)
                End If
        End Select

        basePlane.Name = "Start Puzzle"
        basePlane.Visible = False

        Dim SliceIndex As Integer
        Dim SlicePlane As WorkPlane
        Dim SliceOffset As Double
        Dim SlicePosition As Double
        Dim Ranking As String

        For SliceIndex = 1 To NumberOfSlices.Value
            Ranking = "P"
            SliceOffset = (SliceIndex - 1) * _Spacing
            SlicePlane = _CompDef.WorkPlanes.AddByPlaneAndOffset(basePlane, SliceOffset)
            SlicePlane.Visible = False
            SlicePosition = mainposition + SliceOffset
            CreateSlice(SlicePlane, baseBody, Ranking, SliceIndex, SlicePosition)
        Next

        For SliceIndex = 1 To _SecondarySlicesNumber
            Ranking = "S"
            SliceOffset = (SliceIndex - 1) * _Spacing
            SlicePlane = _CompDef.WorkPlanes.AddByPlaneAndOffset(_secondaryPlane, SliceOffset)
            SlicePlane.Visible = False
            SlicePosition = secondarypostion + SliceOffset
            CreateSlice(SlicePlane, baseBody, Ranking, SliceIndex, SlicePosition)
        Next

        'Toggle next line to get plate class counts
        'MsgBox("Number of main plates = " & _PrimPlates.Count & vbNewLine & "Number of secondary plates = " & _SeconPlates.Count)

    End Sub

    Sub boundingboxcheck(basebody As SurfaceBody)

        Dim boundingboxXmin As WorkPlane
        Dim boundingboxXmax As WorkPlane
        Dim boundingboxYmin As WorkPlane
        Dim boundingboxYmax As WorkPlane
        Dim boundingboxZmin As WorkPlane
        Dim boundingboxZmax As WorkPlane

        boundingboxXmin = _CompDef.WorkPlanes.AddByPlaneAndOffset(_CompDef.WorkPlanes.Item(1), basebody.RangeBox.MinPoint.X)
        boundingboxXmax = _CompDef.WorkPlanes.AddByPlaneAndOffset(_CompDef.WorkPlanes.Item(1), basebody.RangeBox.MaxPoint.X)
        boundingboxYmin = _CompDef.WorkPlanes.AddByPlaneAndOffset(_CompDef.WorkPlanes.Item(2), basebody.RangeBox.MinPoint.Y)
        boundingboxYmax = _CompDef.WorkPlanes.AddByPlaneAndOffset(_CompDef.WorkPlanes.Item(2), basebody.RangeBox.MaxPoint.Y)
        boundingboxZmin = _CompDef.WorkPlanes.AddByPlaneAndOffset(_CompDef.WorkPlanes.Item(3), basebody.RangeBox.MinPoint.Z)
        boundingboxZmax = _CompDef.WorkPlanes.AddByPlaneAndOffset(_CompDef.WorkPlanes.Item(3), basebody.RangeBox.MaxPoint.Z)

        boundingboxXmin.Name = "Xmin"
        boundingboxXmax.Name = "Xmax"
        boundingboxYmin.Name = "Ymin"
        boundingboxYmax.Name = "Ymax"
        boundingboxZmin.Name = "Zmin"
        boundingboxZmax.Name = "Zmax"

        boundingboxXmin.Visible = False
        boundingboxXmax.Visible = False
        boundingboxYmin.Visible = False
        boundingboxYmax.Visible = False
        boundingboxZmin.Visible = False
        boundingboxZmax.Visible = False



    End Sub

    Function SecondaryStart(BaseBody As SurfaceBody, planenumber As Integer) As Double

        Dim width As Double
        Dim secondaryOffset As Double
        Dim secondarypostion As Double
        Dim Maxpoint As Double
        Dim Minpoint As Double

        If planenumber = 1 Then
            Maxpoint = BaseBody.RangeBox.MaxPoint.X
            Minpoint = BaseBody.RangeBox.MinPoint.X
        ElseIf planenumber = 2 Then
            Maxpoint = BaseBody.RangeBox.MaxPoint.Y
            Minpoint = BaseBody.RangeBox.MinPoint.Y
        Else
            Maxpoint = BaseBody.RangeBox.MaxPoint.Z
            Minpoint = BaseBody.RangeBox.MinPoint.Z
        End If

        width = Maxpoint - Minpoint

        'Calculation of the secondary start plane
        _SecondarySlicesNumber = Math.Floor(width / _Spacing)
        secondaryOffset = (width - (_SecondarySlicesNumber - 1) * _Spacing) / 2
        secondarypostion = Minpoint + secondaryOffset
        _secondaryPlane = _CompDef.WorkPlanes.AddByPlaneAndOffset(_CompDef.WorkPlanes.Item(planenumber), secondarypostion)

        _secondaryPlane.Visible = False
        _secondaryPlane.Name = "Secondary Start"

        Return secondarypostion
    End Function

    Sub CreateSlice(SlicePlane As WorkPlane, Body As SurfaceBody, Ranking As String, SliceIndex As Integer, position As Double)
        Dim NewPart As Boolean = True
        Dim contoursketch As PlanarSketch = _CompDef.Sketches.Add(SlicePlane)
        Dim ExtrudeThickness = CDbl(SliceThickness.Value)
        contoursketch.ProjectedCuts.Add()

        Dim exturdeprofiletest As Profile
        exturdeprofiletest = contoursketch.Profiles.AddForSolid(True)
        Debug.Print(exturdeprofiletest.Count)
        Dim FirstSolidProfile As Integer = _CompDef.SurfaceBodies.Count + 1

        If exturdeprofiletest.Count = 1 Then
            Dim extrudetest As ExtrudeDefinition
            extrudetest = _CompDef.Features.ExtrudeFeatures.CreateExtrudeDefinition(exturdeprofiletest, PartFeatureOperationEnum.kNewBodyOperation)
            Call extrudetest.SetDistanceExtent(ExtrudeThickness, PartFeatureExtentDirectionEnum.kSymmetricExtentDirection)
            _CompDef.Features.ExtrudeFeatures.Add(extrudetest)
            _CompDef.SurfaceBodies.Item(_BodyIndex).Name = Ranking & (SliceIndex)
            _CompDef.SurfaceBodies.Item(_BodyIndex).Visible = False
            If Ranking = "P" Then
                Dim plateP As New Plate(position, False, _CompDef.SurfaceBodies.Item(_BodyIndex), False, SlicePlane, False, PartFeatureExtentDirectionEnum.kPositiveExtentDirection)
                _PrimPlates.Add(plateP)
            Else
                Dim plateS As New Plate(position, False, _CompDef.SurfaceBodies.Item(_BodyIndex), False, SlicePlane, False, PartFeatureExtentDirectionEnum.kPositiveExtentDirection)
                _SeconPlates.Add(plateS)
            End If
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
                If i = 1 Then
                    'Create first solid
                    extrudetest = _CompDef.Features.ExtrudeFeatures.CreateExtrudeDefinition(newprof, PartFeatureOperationEnum.kNewBodyOperation)
                    Call extrudetest.SetDistanceExtent(ExtrudeThickness, PartFeatureExtentDirectionEnum.kSymmetricExtentDirection)
                    _CompDef.Features.ExtrudeFeatures.Add(extrudetest)
                    _CompDef.SurfaceBodies.Item(_BodyIndex).Name = Ranking & (SliceIndex) & "-" & i
                    _CompDef.SurfaceBodies.Item(_BodyIndex).Visible = False

                Else
                    'Try to use the second profile to remove a volume
                    Dim startvolume As Double = _CompDef.SurfaceBodies.Item(FirstSolidProfile).Volume(95)
                    extrudetest = _CompDef.Features.ExtrudeFeatures.CreateExtrudeDefinition(newprof, PartFeatureOperationEnum.kCutOperation)
                    Dim firstprofilecol As ObjectCollection = _invApp.TransientObjects.CreateObjectCollection
                    Call firstprofilecol.Add(_CompDef.SurfaceBodies.Item(FirstSolidProfile))
                    extrudetest.AffectedBodies = firstprofilecol
                    Call extrudetest.SetDistanceExtent(ExtrudeThickness, PartFeatureExtentDirectionEnum.kSymmetricExtentDirection)
                    _CompDef.Features.ExtrudeFeatures.Add(extrudetest)

                    'Check if the cut operation changed the existing solid
                    If startvolume = _CompDef.SurfaceBodies.Item(FirstSolidProfile).Volume(95) Then

                        'Create a new solid with the new loop
                        extrudetest = _CompDef.Features.ExtrudeFeatures.CreateExtrudeDefinition(newprof, PartFeatureOperationEnum.kNewBodyOperation)
                        Call extrudetest.SetDistanceExtent(ExtrudeThickness, PartFeatureExtentDirectionEnum.kSymmetricExtentDirection)
                        _CompDef.Features.ExtrudeFeatures.Add(extrudetest)
                        _CompDef.SurfaceBodies.Item(_BodyIndex).Name = Ranking & (SliceIndex) & "-" & i
                        _CompDef.SurfaceBodies.Item(_BodyIndex).Visible = False

                        'The cutfeature is only used as a test and should be removed
                        _CompDef.Features.Item(_CompDef.Features.Count - 1).Delete(False, False, False)
                    Else
                        NewPart = False
                    End If

                End If
                If NewPart Then
                    If Ranking = "P" Then
                        Dim plateP As New Plate(position, False, _CompDef.SurfaceBodies.Item(_BodyIndex), False, SlicePlane, False, PartFeatureExtentDirectionEnum.kPositiveExtentDirection)
                        _PrimPlates.Add(plateP)
                    Else
                        Dim plateS As New Plate(position, False, _CompDef.SurfaceBodies.Item(_BodyIndex), False, SlicePlane, False, PartFeatureExtentDirectionEnum.kPositiveExtentDirection)
                        _SeconPlates.Add(plateS)
                    End If
                    _BodyIndex = _BodyIndex + 1
                End If
            Next
        End If

        contoursketch.Visible = False

        If Ranking = "P" Then
            _PrimExtra = exturdeprofiletest.Count - 1

        Else
            _SecondaryExtra = exturdeprofiletest.Count - 1
        End If

    End Sub

    Sub makebodiesvisible()

        For Each Pplate As Plate In _PrimPlates
            Dim surf As Inventor.SurfaceBody = Pplate.PlateSurfBodyID
            surf.Visible = True
        Next

        For Each Splate As Plate In _SeconPlates
            Dim surf As SurfaceBody = Splate.PlateSurfBodyID
            surf.Visible = True
        Next

        Dim previndex As Integer
        For previndex = 1 To _initialComp
            _CompDef.SurfaceBodies.Item(previndex).Visible = False
        Next

        If _Colormode Then
            'Colour first Prim Body
            Dim FirstPrimBody As SurfaceBody = _PrimPlates.Item(0).PlateSurfBodyID
            Dim oFace As Face
            For Each oFace In FirstPrimBody.Faces
                ' Set the render style to be "As Feature". 
                Call oFace.SetRenderStyle(StyleSourceTypeEnum.kOverrideRenderStyle, _Doc.RenderStyles.Item("Smooth - Dark Forest Green"))
            Next

            'Colour first Secondary Body
            Dim FirstSecondary As SurfaceBody = _SeconPlates.Item(0).PlateSurfBodyID
            For Each oFace In FirstSecondary.Faces
                ' Set the render style to be "As Feature". 
                Call oFace.SetRenderStyle(StyleSourceTypeEnum.kOverrideRenderStyle, _Doc.RenderStyles.Item("Smooth - Red"))
            Next
        End If
    End Sub

    '*********** Create Intersections ********

    Sub CreateIntersections()
        Dim PrimIndex As Integer = 0
        Dim SecIndex As Integer = 0
        Dim progressval As Integer = 0
        Dim progressprocent As Double

        My.Forms.ProgressForm.Show()

        For Each MPlate As Plate In _PrimPlates
            SecIndex = 0
            For Each SPlate As Plate In _SeconPlates
                CreateIntersection(MPlate, SPlate)
                Debug.Print("Mplate = " & MPlate.PlateSurfBodyID.Name & " Splate = " & SPlate.PlateSurfBodyID.Name)
                SecIndex = SecIndex + 1
                progressval = PrimIndex * _SeconPlates.Count + SecIndex
                progressprocent = progressval / (_PrimPlates.Count * _SeconPlates.Count)
                My.Forms.ProgressForm.ProgressBar1.Value = CInt(progressprocent * My.Forms.ProgressForm.ProgressBar1.Maximum)
                Debug.Print(progressprocent)
            Next
            PrimIndex = PrimIndex + 1
        Next

        For Each MPlate In _PrimPlates
            If Not MPlate.PlateHasCuts Then
                MPlate.PlateSurfBodyID.Visible = False
            End If
        Next

        For Each Splate In _SeconPlates
            If Not Splate.PlateHasCuts Then
                Splate.PlateSurfBodyID.Visible = False
            End If
        Next

        My.Forms.ProgressForm.Close()
    End Sub

    Sub CreateIntersection(BasePlate As Plate, ToolPlate As Plate)

        Dim TransBody As TransientBRep = _invApp.TransientBRep
        Dim transGeo As TransientGeometry = _invApp.TransientGeometry
        Dim objs As ObjectCollection = _invApp.TransientObjects.CreateObjectCollection

        'Create duplicate bodies
        Dim transBase As SurfaceBody = TransBody.Copy(BasePlate.PlateSurfBodyID)
        Dim transTool As SurfaceBody = TransBody.Copy(ToolPlate.PlateSurfBodyID)
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

        BasePlate.PlateHasCuts = True
        ToolPlate.PlateHasCuts = True

        'Create plane
        Dim LatestBody As SurfaceBody = _CompDef.SurfaceBodies.Item(_CompDef.SurfaceBodies.Count)
        Dim splitDist As Double
        'Dim WorkPts As WorkPoints = _CompDef.WorkPoints
        Dim WorkPlns As WorkPlanes = _CompDef.WorkPlanes
        Dim WorkPts As WorkPoints = _CompDef.WorkPoints
        Dim splitplane As WorkPlane

        'Create Sketches
        Dim Centerpoint As Point2d = _invApp.TransientGeometry.CreatePoint2d
        Dim Cornerpoint As Point2d = _invApp.TransientGeometry.CreatePoint2d

        Select Case _SplitDir
            Case 1
                splitDist = (LatestBody.RangeBox.MaxPoint.X - LatestBody.RangeBox.MinPoint.X) / 2 + LatestBody.RangeBox.MinPoint.X
                splitplane = WorkPlns.AddByPlaneAndOffset(_CompDef.WorkPlanes.Item(1), splitDist)

            Case 2
                splitDist = (LatestBody.RangeBox.MaxPoint.Y - LatestBody.RangeBox.MinPoint.Y) / 2 + LatestBody.RangeBox.MinPoint.Y
                splitplane = WorkPlns.AddByPlaneAndOffset(_CompDef.WorkPlanes.Item(2), splitDist)

            Case Else
                splitDist = (LatestBody.RangeBox.MaxPoint.Z - LatestBody.RangeBox.MinPoint.Z) / 2 + LatestBody.RangeBox.MinPoint.Z
                splitplane = WorkPlns.AddByPlaneAndOffset(_CompDef.WorkPlanes.Item(3), splitDist)

        End Select

        splitplane.Visible = False

        'Delete latest body feature
        _CompDef.Features.NonParametricBaseFeatures.Item(1).Delete()

        'Set toolplate cut direction
        Dim basecutdir As PartFeatureExtentDirectionEnum

        If Not ToolPlate.PlateHasCutDir Then
            If splitDist <= _BaseBodyCenter Then
                ToolPlate.PlateCutDir = PartFeatureExtentDirectionEnum.kNegativeExtentDirection
                basecutdir = PartFeatureExtentDirectionEnum.kPositiveExtentDirection
            Else
                ToolPlate.PlateCutDir = PartFeatureExtentDirectionEnum.kPositiveExtentDirection
                basecutdir = PartFeatureExtentDirectionEnum.kNegativeExtentDirection
            End If
            ToolPlate.PlateHasCutDir = True
        Else
            If ToolPlate.PlateCutDir = PartFeatureExtentDirectionEnum.kNegativeExtentDirection Then
                basecutdir = PartFeatureExtentDirectionEnum.kPositiveExtentDirection
            Else
                basecutdir = PartFeatureExtentDirectionEnum.kNegativeExtentDirection
            End If
        End If

        'Create point
        Dim CenterWorkPoint As WorkPoint = WorkPts.AddByThreePlanes(splitplane, ToolPlate.Plateplane, BasePlate.Plateplane)
        CenterWorkPoint.Visible = False

        'BaseSketch
        Dim BaseSketch As PlanarSketch = _CompDef.Sketches.Add(splitplane)
        BaseSketch.AddByProjectingEntity(CenterWorkPoint)

        Dim newcorner As Point2d = _invApp.TransientGeometry.CreatePoint2d
        newcorner.X = BaseSketch.SketchPoints.Item(1).Geometry.X + SliceThickness.Value / 2
        newcorner.Y = BaseSketch.SketchPoints.Item(1).Geometry.Y + SliceThickness.Value / 2
        BaseSketch.SketchLines.AddAsTwoPointCenteredRectangle(BaseSketch.SketchPoints.Item(1), newcorner)

        'Create cut in Baseplate
        Dim BaseProfile As Profile = BaseSketch.Profiles.AddForSolid()
        Dim BaseExtDef As ExtrudeDefinition = _CompDef.Features.ExtrudeFeatures.CreateExtrudeDefinition(BaseProfile, PartFeatureOperationEnum.kCutOperation)

        Dim Basecoll As ObjectCollection = _invApp.TransientObjects.CreateObjectCollection
        Call Basecoll.Add(ToolPlate.PlateSurfBodyID)
        BaseExtDef.AffectedBodies = Basecoll
        Call BaseExtDef.SetThroughAllExtent(basecutdir)
        _CompDef.Features.ExtrudeFeatures.Add(BaseExtDef)

        'ToolSketch needed for cut corrections, not necesary yet
        'Dim ToolSketch As PlanarSketch = _CompDef.Sketches.Add(splitplane)
        'ToolSketch.SketchLines.AddAsTwoPointCenteredRectangle(Centerpoint, Cornerpoint)

        'Create cut in Toolplate
        Dim ToolProfile As Profile = BaseSketch.Profiles.AddForSolid()
        Dim ToolExtDef As ExtrudeDefinition = _CompDef.Features.ExtrudeFeatures.CreateExtrudeDefinition(ToolProfile, PartFeatureOperationEnum.kCutOperation)

        Dim Toolcoll As ObjectCollection = _invApp.TransientObjects.CreateObjectCollection
        Call Toolcoll.Add(BasePlate.PlateSurfBodyID)
        ToolExtDef.AffectedBodies = Toolcoll
        Call ToolExtDef.SetThroughAllExtent(ToolPlate.PlateCutDir)
        _CompDef.Features.ExtrudeFeatures.Add(ToolExtDef)

    End Sub

    '************* DXF Export ************

    Sub DXFExport()

        CreateFolder()
        For Each PPlate In _PrimPlates
            If PPlate.PlateHasCuts Then
                HideAllButCurrent(PPlate)
                CreateDXF(PPlate)
            End If
        Next

        For Each SPlate In _SeconPlates
            If SPlate.PlateHasCuts Then
                HideAllButCurrent(SPlate)
                CreateDXF(SPlate)
            End If
        Next

        For Each MPlate In _PrimPlates
            If MPlate.PlateHasCuts Then
                MPlate.PlateSurfBodyID.Visible = True
            Else
                MPlate.PlateSurfBodyID.Visible = False
            End If
        Next

        For Each Splate In _SeconPlates
            If Splate.PlateHasCuts Then
                Splate.PlateSurfBodyID.Visible = True
            Else
                Splate.PlateSurfBodyID.Visible = False
            End If
        Next

    End Sub

    Sub CreateFolder()

        _invApp.ActiveDocument.Save()

        Dim path As String = System.IO.Path.GetDirectoryName(_Doc.FullFileName)
        Dim filename As String = Replace(_Doc.FullFileName, path, "")
        Dim name As String = Replace(filename, ".ipt", "")

        _Filelocation = path + name

        If (System.IO.Directory.Exists(_Filelocation)) Then
            Dim ToD As String = TimeOfDay.ToShortTimeString
            ToD = Replace(ToD, ":", "-")
            _Filelocation = _Filelocation + " " + ToD
            System.IO.Directory.CreateDirectory(_Filelocation)
        Else
            System.IO.Directory.CreateDirectory(_Filelocation)
        End If

    End Sub

    Private Sub HideAllButCurrent(toolbody As Plate)
        For Each body As SurfaceBody In _CompDef.SurfaceBodies
            If body.Name = toolbody.PlateSurfBodyID.Name Then
                body.Visible = True
            Else
                body.Visible = False
            End If
        Next
    End Sub

    Private Sub CreateDXF(toolbody As Plate)
        'create sketch with cut edges
        Dim contoursketch As PlanarSketch = _CompDef.Sketches.Add(toolbody.Plateplane)
        contoursketch.ProjectedCuts.Add()

        'export sketch to dxf
        Call contoursketch.DataIO.WriteDataToFile("DXF", _Filelocation & "\" & toolbody.PlateSurfBodyID.Name & ".dxf")

        'rename and hide sketch
        contoursketch.Name = "DXF-" & toolbody.PlateSurfBodyID.Name
        contoursketch.Visible = False
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

Public Class Plate

    Public PlatePosition As Double
    Public PlateHasCuts As Boolean
    Public PlateSurfBodyID As SurfaceBody
    Public PlateSecondary As Boolean
    Public Plateplane As WorkPlane
    Public PlateHasCutDir As Boolean
    Public PlateCutDir As PartFeatureExtentDirectionEnum

    Public Sub New()

    End Sub

    Public Sub New(ByVal Position As Double, ByVal HasCuts As Boolean, ByVal SurfBodyID As SurfaceBody, ByVal Secondary As Boolean, ByVal sketchplane As WorkPlane, ByVal hascutdir As Boolean, ByVal cutdir As PartFeatureExtentDirectionEnum)
        PlatePosition = Position
        PlateHasCuts = HasCuts
        PlateSurfBodyID = SurfBodyID
        PlateSecondary = Secondary
        Plateplane = sketchplane
        PlateHasCutDir = hascutdir
        PlateCutDir = cutdir
    End Sub

End Class
