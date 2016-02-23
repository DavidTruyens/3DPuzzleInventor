
Imports System
Imports System.Type
Imports System.Activator
Imports System.Threading
Imports System.Runtime.InteropServices
Imports Inventor
Imports System.IO


Public Class Form1
    Public Shared _invApp As Inventor.Application
    Dim _Doc As Inventor.PartDocument
    Dim _OrigDoc As Inventor.PartDocument
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
    Dim _Scaled As Boolean = False
    Dim _TargetVolume As Double = 5000
    Dim _ToolDiam As Double = 0.02
    Dim _Toolcomp As Boolean = True
    Dim _Nest As Boolean = True

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

        ' My.Forms.Form1.TopMost = True

    End Sub

    '************ Start Puzzle ***********

    Private Sub GetBodyButton_Click(sender As Object, e As EventArgs) Handles GetBodyButton.Click

        _OrigDoc = _invApp.ActiveDocument

        'Get the actual surface body
        Dim body As SurfaceBody = GetBody()

        'Check if the model is already scaled. If not a derived part will be created and scaled
        Dim startbody As SurfaceBody = CreateDerived(body)

        If startbody IsNot Nothing Then

            'Calculates the longest value of the boundingbox and sets that as the direction for the primary plates
            Dim MainDir As KeyValuePair(Of Integer, Double) = GetBoundingBoxLength(startbody)

            _initialComp = _CompDef.SurfaceBodies.Count

            'only needed to see the deviation of the boundingbox with freeform models
            If _DebugMode Then
                boundingboxcheck(startbody)
            End If

            'Createion of the slices in two directions
            GenerateSlices(MainDir, startbody)

            'Makes components visible and colors the first plates in each direction
            makebodiesvisible()

            'Creates intesections in plates. Plates who don't have an intersection will be turned invisible
            CreateIntersections()

            MsgBox("Plate creation finished. Check the results, make changes if needed and click the DXF button!")

            DXFButton.Enabled = True
        End If

    End Sub

    Private Sub DXFButton_Click(sender As Object, e As EventArgs) Handles DXFButton.Click
        DXFExport()

        Dim FileMsg As DialogResult
        FileMsg = MsgBox("All contours have been exported and can be found here:" & vbNewLine & _Filelocation & vbNewLine & "Click YES to go to the file location", MsgBoxStyle.YesNo)

        If FileMsg = DialogResult.Yes Then
            Process.Start(_Filelocation)
        End If

        My.Forms.Form1.Close()
    End Sub

    Private Sub DeleteButton_Click(sender As Object, e As EventArgs) Handles DeleteButton.Click
        DeletePuzzle()
    End Sub

    Private Function GetBody() As SurfaceBody
        ' Have the bodies selected. 
        Dim initialBody As SurfaceBody
        initialBody = _invApp.CommandManager.Pick(SelectionFilterEnum.kPartBodyFilter, "Select the base body")
        Return initialBody
    End Function

    Private Function CreateDerived(body As SurfaceBody) As SurfaceBody

        If _OrigDoc.ComponentDefinition.ReferenceComponents.DerivedPartComponents.Count = 1 Then
            _Doc = _OrigDoc
            _CompDef = _Doc.ComponentDefinition

            'Check if a puzzle already exist. It should be deleted before you can continue
            If Puzzletest() Then
                Dim msg1res As DialogResult
                msg1res = MsgBox("A puzzle was already created, would you like to delete it?", MsgBoxStyle.YesNo, "Puzzletest") '& MsgBoxStyle.SystemModal, "On Top"
                If msg1res = DialogResult.Yes Then
                    DeletePuzzle()
                Else
                    Return Nothing
                    Exit Function
                End If
            End If

        Else
            Dim origindoc As PartDocument = _invApp.ActiveDocument
            origindoc.Save()

            Dim origVol As Double = body.Volume(95)

            ' Create a new part file to derive the selected part into 
            'note: kPartDocumentObject is the default template
            Dim NewPrt As PartDocument
            NewPrt = _invApp.Documents.Add(DocumentTypeEnum.kPartDocumentObject, _invApp.FileManager.GetTemplateFile(DocumentTypeEnum.kPartDocumentObject))

            'Create a derived definition for the selected part
            Dim DerivedPrtDef As DerivedPartUniformScaleDef
            DerivedPrtDef = NewPrt.ComponentDefinition.ReferenceComponents.DerivedPartComponents.CreateUniformScaleDef(origindoc.FullFileName)

            Call DerivedPrtDef.ExcludeAll()

            Dim i As Integer = 0
            For Each surfding As SurfaceBody In _OrigDoc.ComponentDefinition.SurfaceBodies
                i = i + 1
                If surfding.Name = body.Name Then
                    Exit For
                End If
            Next
            DerivedPrtDef.Solids.Item(i).IncludeEntity = True

            ' set the scale to use
            DerivedPrtDef.ScaleFactor = Math.Pow(_TargetVolume / origVol, 1 / 3)

            ' Create the derived part.
            NewPrt.ComponentDefinition.ReferenceComponents.DerivedPartComponents.Add(DerivedPrtDef)
            NewPrt.Views.Item(1).GoHome()

            Dim origname As String = origindoc.FullFileName
            Dim name As String = Replace(origname, ".ipt", "")
            Dim newname As String = name + "-scaled.ipt"

            If (System.IO.File.Exists(newname)) Then
                Dim ToD As String = TimeOfDay.ToShortTimeString
                ToD = Replace(ToD, ":", "-")
                newname = Replace(newname, ".ipt", "")
                newname = newname + " " + ToD + ".ipt"
            End If

            NewPrt.SaveAs(newname, False)
            _Doc = NewPrt
            _CompDef = _Doc.ComponentDefinition
        End If

        Return _CompDef.SurfaceBodies.Item(1)

    End Function

    Private Function GetBoundingBoxLength(boxbody As SurfaceBody) As KeyValuePair(Of Integer, Double)
        Dim BoundingBox As Box
        Dim MainDir As Integer
        BoundingBox = boxbody.RangeBox

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

    Private Function Puzzletest() As Boolean

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

    Private Sub DeletePuzzle()

        Dim BrsPanes As BrowserPanes = _invApp.ActiveDocument.BrowserPanes
        Dim BrsPan As Inventor.BrowserPane = (From _Pan As BrowserPane In BrsPanes Where _Pan.TreeBrowser Select _Pan).FirstOrDefault
        Dim DoDelete As Boolean = False
        Dim ObjCollection As ObjectCollection = _invApp.TransientObjects.CreateObjectCollection()

        For Each wkpln As WorkPlane In _CompDef.WorkPlanes
            If wkpln.Name = "Start Puzzle" Then
                wkpln.SetEndOfPart(True)
                Exit For
            End If
        Next

        Dim godelete As Boolean = True
        Dim objCounter As Integer = 0

        While godelete
            Dim _node As BrowserNode = BrsPan.TopNode.BrowserNodes.Item(BrsPan.TopNode.BrowserNodes.Count)
            If _node.BrowserNodeDefinition.Label = ("End of Part") Then
                godelete = False
            Else
                Dim objtest = _node.NativeObject
                ObjCollection.Add(objtest)
                _CompDef.DeleteObjects(ObjCollection)
                ObjCollection.Clear()
            End If
        End While

        For Each obody As SurfaceBody In _CompDef.SurfaceBodies
            obody.Visible = True
        Next
    End Sub

    '*********** Generate Slices ************

    Private Sub GenerateSlices(MainDir As KeyValuePair(Of Integer, Double), baseBody As SurfaceBody)
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

    Private Sub boundingboxcheck(basebody As SurfaceBody)

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

    Private Function SecondaryStart(BaseBody As SurfaceBody, planenumber As Integer) As Double

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

    Private Sub CreateSlice(SlicePlane As WorkPlane, Body As SurfaceBody, Ranking As String, SliceIndex As Integer, position As Double)
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
            Try
                _CompDef.Features.ExtrudeFeatures.Add(extrudetest)
            Catch ex As Exception
                Exit Sub
            End Try

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
                    Try
                        _CompDef.Features.ExtrudeFeatures.Add(extrudetest)
                    Catch ex As Exception
                        Exit Sub
                    End Try

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

    Private Sub makebodiesvisible()

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

    Private Sub CreateIntersections()
        Dim PrimIndex As Integer = 0
        Dim SecIndex As Integer = 0
        Dim progressval As Integer = 0
        Dim progressprocent As Double

        My.Forms.ProgressForm.Show()
        My.Forms.ProgressForm.TopMost = True

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
        My.Forms.Form1.TopMost = True
        My.Forms.Form1.TopMost = False
    End Sub

    Private Sub CreateIntersection(BasePlate As Plate, ToolPlate As Plate)

        Dim TransBody As TransientBRep = _invApp.TransientBRep
        Dim transGeo As TransientGeometry = _invApp.TransientGeometry
        Dim objs As ObjectCollection = _invApp.TransientObjects.CreateObjectCollection
        Dim hasdirection As Boolean = False

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

        Catch ex As Exception
            Exit Sub
        End Try

        BasePlate.PlateHasCuts = True
        ToolPlate.PlateHasCuts = True

        'Set toolplate cut direction
        Dim basecutdir As PartFeatureExtentDirectionEnum
        Dim toolcutdir As PartFeatureExtentDirectionEnum

        For Each transbool As FaceShell In transBase.FaceShells

            'Create plane
            Dim splitDist As Double
            Dim WorkPlns As WorkPlanes = _CompDef.WorkPlanes
            Dim WorkPts As WorkPoints = _CompDef.WorkPoints
            Dim splitplane As WorkPlane

            'Create Sketches
            Dim Centerpoint As Point2d = _invApp.TransientGeometry.CreatePoint2d
            Dim Cornerpoint As Point2d = _invApp.TransientGeometry.CreatePoint2d
            Dim Conerpoint2 As Point2d = _invApp.TransientGeometry.CreatePoint2d

            'Platecenters
            Dim basebodycenter As Double
            Dim toolbodycenter As Double

            'Extrude length
            Dim extrudelength As Double

            'Compenations
            Dim XBaseComp As Double
            Dim YBaseComp As Double
            Dim XToolComp As Double
            Dim YToolComp As Double

            Select Case _SplitDir
                Case 1
                    'plane creation and extrude distance
                    splitDist = (transbool.RangeBox.MaxPoint.X - transbool.RangeBox.MinPoint.X) / 2 + transbool.RangeBox.MinPoint.X
                    splitplane = WorkPlns.AddByPlaneAndOffset(_CompDef.WorkPlanes.Item(1), splitDist)
                    basebodycenter = BasePlate.PlateSurfBodyID.RangeBox.MinPoint.X + (BasePlate.PlateSurfBodyID.RangeBox.MaxPoint.X - BasePlate.PlateSurfBodyID.RangeBox.MinPoint.X) / 2
                    toolbodycenter = ToolPlate.PlateSurfBodyID.RangeBox.MinPoint.X + (ToolPlate.PlateSurfBodyID.RangeBox.MaxPoint.X - ToolPlate.PlateSurfBodyID.RangeBox.MinPoint.X) / 2
                    extrudelength = (transbool.RangeBox.MaxPoint.X - transbool.RangeBox.MinPoint.X) / 2 + SliceThickness.Value * 4

                    'compensation calculation
                    XBaseComp = _ToolDiam / 2
                    YBaseComp = 0
                    XToolComp = 0
                    YToolComp = _ToolDiam / 2

                Case 2
                    splitDist = (transbool.RangeBox.MaxPoint.Y - transbool.RangeBox.MinPoint.Y) / 2 + transbool.RangeBox.MinPoint.Y
                    splitplane = WorkPlns.AddByPlaneAndOffset(_CompDef.WorkPlanes.Item(2), splitDist)
                    basebodycenter = BasePlate.PlateSurfBodyID.RangeBox.MinPoint.Y + (BasePlate.PlateSurfBodyID.RangeBox.MaxPoint.Y - BasePlate.PlateSurfBodyID.RangeBox.MinPoint.Y) / 2
                    toolbodycenter = ToolPlate.PlateSurfBodyID.RangeBox.MinPoint.Y + (ToolPlate.PlateSurfBodyID.RangeBox.MaxPoint.Y - ToolPlate.PlateSurfBodyID.RangeBox.MinPoint.Y) / 2
                    extrudelength = (transbool.RangeBox.MaxPoint.Y - transbool.RangeBox.MinPoint.Y) / 2 + SliceThickness.Value * 4

                    'compensation calculation
                    XBaseComp = _ToolDiam / 2
                    YBaseComp = 0
                    XToolComp = 0
                    YToolComp = _ToolDiam / 2
                Case Else
                    splitDist = (transbool.RangeBox.MaxPoint.Z - transbool.RangeBox.MinPoint.Z) / 2 + transbool.RangeBox.MinPoint.Z
                    splitplane = WorkPlns.AddByPlaneAndOffset(_CompDef.WorkPlanes.Item(3), splitDist)
                    basebodycenter = BasePlate.PlateSurfBodyID.RangeBox.MinPoint.Z + (BasePlate.PlateSurfBodyID.RangeBox.MaxPoint.Z - BasePlate.PlateSurfBodyID.RangeBox.MinPoint.Z) / 2
                    toolbodycenter = ToolPlate.PlateSurfBodyID.RangeBox.MinPoint.Z + (ToolPlate.PlateSurfBodyID.RangeBox.MaxPoint.Z - ToolPlate.PlateSurfBodyID.RangeBox.MinPoint.Z) / 2
                    extrudelength = (transbool.RangeBox.MaxPoint.Z - transbool.RangeBox.MinPoint.Z) / 2 + SliceThickness.Value * 4

                    'compensation calculation
                    XBaseComp = 0
                    YBaseComp = _ToolDiam / 2
                    XToolComp = _ToolDiam / 2
                    YToolComp = 0
            End Select

            splitplane.Visible = False

            If Not hasdirection Then
                If Math.Abs(_BaseBodyCenter - basebodycenter) + 0.05 >= Math.Abs(_BaseBodyCenter - toolbodycenter) Then
                    If basebodycenter + 0.05 - _BaseBodyCenter >= 0 Then
                        basecutdir = PartFeatureExtentDirectionEnum.kPositiveExtentDirection
                        toolcutdir = PartFeatureExtentDirectionEnum.kNegativeExtentDirection
                    Else
                        basecutdir = PartFeatureExtentDirectionEnum.kNegativeExtentDirection
                        toolcutdir = PartFeatureExtentDirectionEnum.kPositiveExtentDirection
                    End If
                Else
                    If toolbodycenter + 0.05 - _BaseBodyCenter >= 0 Then
                        basecutdir = PartFeatureExtentDirectionEnum.kNegativeExtentDirection
                        toolcutdir = PartFeatureExtentDirectionEnum.kPositiveExtentDirection
                    Else
                        basecutdir = PartFeatureExtentDirectionEnum.kPositiveExtentDirection
                        toolcutdir = PartFeatureExtentDirectionEnum.kNegativeExtentDirection
                    End If
                End If
            End If
            
            'Create point
            Dim CenterWorkPoint As WorkPoint = WorkPts.AddByThreePlanes(splitplane, ToolPlate.Plateplane, BasePlate.Plateplane)
            CenterWorkPoint.Visible = False

            'Toolcompensation will create two different skechtes where the tooldiam is reduced. Otherwise the exact cutout will be calculated
            If _Toolcomp Then
                'BaseSketch
                Dim BaseSketch As PlanarSketch = _CompDef.Sketches.Add(splitplane)
                BaseSketch.AddByProjectingEntity(CenterWorkPoint)

                Dim basecorner As Point2d = _invApp.TransientGeometry.CreatePoint2d
                basecorner.X = BaseSketch.SketchPoints.Item(1).Geometry.X + SliceThickness.Value / 2 - XBaseComp
                basecorner.Y = BaseSketch.SketchPoints.Item(1).Geometry.Y + SliceThickness.Value / 2 - YBaseComp
                BaseSketch.SketchLines.AddAsTwoPointCenteredRectangle(BaseSketch.SketchPoints.Item(1), basecorner)

                'Create cut in Baseplate
                Dim BaseProfile As Profile = BaseSketch.Profiles.AddForSolid()
                Dim BaseExtDef As ExtrudeDefinition = _CompDef.Features.ExtrudeFeatures.CreateExtrudeDefinition(BaseProfile, PartFeatureOperationEnum.kCutOperation)

                Dim Basecoll As ObjectCollection = _invApp.TransientObjects.CreateObjectCollection
                Call Basecoll.Add(BasePlate.PlateSurfBodyID)
                BaseExtDef.AffectedBodies = Basecoll
                Call BaseExtDef.SetDistanceExtent(extrudelength, basecutdir)
                _CompDef.Features.ExtrudeFeatures.Add(BaseExtDef)

                'ToolSketch
                Dim ToolSketch As PlanarSketch = _CompDef.Sketches.Add(splitplane)
                ToolSketch.AddByProjectingEntity(CenterWorkPoint)

                Dim toolcorner As Point2d = _invApp.TransientGeometry.CreatePoint2d
                toolcorner.X = ToolSketch.SketchPoints.Item(1).Geometry.X + SliceThickness.Value / 2 - XToolComp
                toolcorner.Y = ToolSketch.SketchPoints.Item(1).Geometry.Y + SliceThickness.Value / 2 - YToolComp
                ToolSketch.SketchLines.AddAsTwoPointCenteredRectangle(ToolSketch.SketchPoints.Item(1), toolcorner)

                'Create cut in Toolplate
                Dim ToolProfile As Profile = ToolSketch.Profiles.AddForSolid()
                Dim ToolExtDef As ExtrudeDefinition = _CompDef.Features.ExtrudeFeatures.CreateExtrudeDefinition(ToolProfile, PartFeatureOperationEnum.kCutOperation)

                Dim Toolcoll As ObjectCollection = _invApp.TransientObjects.CreateObjectCollection
                Call Toolcoll.Add(ToolPlate.PlateSurfBodyID)
                ToolExtDef.AffectedBodies = Toolcoll
                Call ToolExtDef.SetDistanceExtent(extrudelength, toolcutdir)
                _CompDef.Features.ExtrudeFeatures.Add(ToolExtDef)

            Else
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
                Call BaseExtDef.SetDistanceExtent(extrudelength, basecutdir)
                _CompDef.Features.ExtrudeFeatures.Add(BaseExtDef)

                'Create cut in Toolplate
                Dim ToolProfile As Profile = BaseSketch.Profiles.AddForSolid()
                Dim ToolExtDef As ExtrudeDefinition = _CompDef.Features.ExtrudeFeatures.CreateExtrudeDefinition(ToolProfile, PartFeatureOperationEnum.kCutOperation)

                Dim Toolcoll As ObjectCollection = _invApp.TransientObjects.CreateObjectCollection
                Call Toolcoll.Add(BasePlate.PlateSurfBodyID)
                ToolExtDef.AffectedBodies = Toolcoll

                Call ToolExtDef.SetDistanceExtent(extrudelength, toolcutdir)
                _CompDef.Features.ExtrudeFeatures.Add(ToolExtDef)
            End If

        Next

    End Sub

    '************* DXF Export ************

    Private Sub DXFExport()

        CreateFolder()
        Dim assy As AssemblyDocument = CreateSMAssy()
        NEST(assy)

    End Sub

    Private Sub CreateFolder()

        _invApp.ActiveDocument.Save()

        Dim path As String = System.IO.Path.GetDirectoryName(_Doc.FullFileName)
        Dim filename As String = Replace(_Doc.FullFileName, path, "")
        Dim name As String = Replace(filename, ".ipt", "")

        _Filelocation = path + name

        If (System.IO.Directory.Exists(_Filelocation)) Then
            Dim ToD As String = TimeOfDay.ToShortTimeString
            ToD = Replace(ToD, ":", "-")
            _Filelocation = _Filelocation + " " + ToD

        End If
        System.IO.Directory.CreateDirectory(_Filelocation)
    End Sub

    Private Function CreateSMAssy() As AssemblyDocument

        'Create New assembly
        Dim assy As AssemblyDocument = _invApp.Documents.Add(DocumentTypeEnum.kAssemblyDocumentObject, "", True)

        ' Set a reference to the transient geometry object.
        Dim oTG As TransientGeometry
        oTG = _invApp.TransientGeometry

        ' Create a matrix.  A new matrix is initialized with an identity matrix.
        Dim oMatrix As Matrix
        oMatrix = oTG.CreateMatrix

        Call oMatrix.SetTranslation(oTG.CreateVector(0, 0, 0), False)

        Dim origname As String = _Doc.FullFileName
        Dim name As String = Replace(origname, ".ipt", "")
        Dim path As String = IO.Path.GetDirectoryName(_Doc.FullFileName)

        Dim assyname As String = name + ".iam"

        If (IO.File.Exists(assyname)) Then
            Dim ToD As String = TimeOfDay.ToShortTimeString
            ToD = Replace(ToD, ":", "-")
            assyname = name + ToD + ".iam"
        End If

        assy.SaveAs(assyname, False)

        Dim i As Integer = 0
        Dim progressval As Integer = 0
        Dim progressprocent As Double

        My.Forms.ProgressForm.Show()
        My.Forms.ProgressForm.TopMost = True

        For i = 1 To _CompDef.SurfaceBodies.Count

            progressprocent = i / _CompDef.SurfaceBodies.Count
            My.Forms.ProgressForm.ProgressBar1.Value = CInt(progressprocent * My.Forms.ProgressForm.ProgressBar1.Maximum)
            Debug.Print(progressprocent)

            If _CompDef.SurfaceBodies.Item(i).Visible = True Then
                ' _invApp.ScreenUpdating = False
                Dim NewPrt As PartDocument
                NewPrt = _invApp.Documents.Add(DocumentTypeEnum.kPartDocumentObject,
                         _invApp.FileManager.GetTemplateFile(DocumentTypeEnum.kPartDocumentObject,
                                                             SystemOfMeasureEnum.kDefaultSystemOfMeasure,
                                                             DraftingStandardEnum.kDefault_DraftingStandard,
                                                             "{9C464203-9BAE-11D3-8BAD-0060B0CE6BB4}"), False)

                'Create a derived definition for the selected part
                Dim DerivedPrtDef As DerivedPartUniformScaleDef
                DerivedPrtDef = NewPrt.ComponentDefinition.ReferenceComponents.DerivedPartComponents.CreateUniformScaleDef(_Doc.FullFileName)
                Call DerivedPrtDef.ExcludeAll()
                DerivedPrtDef.Solids.Item(i).IncludeEntity = True

                ' Create the derived part.
                NewPrt.ComponentDefinition.ReferenceComponents.DerivedPartComponents.Add(DerivedPrtDef)

                Dim SMCompdef As SheetMetalComponentDefinition = NewPrt.ComponentDefinition
                SMCompdef.UseSheetMetalStyleThickness = False
                SMCompdef.Thickness.Value = SliceThickness.Value

                Dim newname As String = _Filelocation + "\" + _CompDef.SurfaceBodies.Item(i).Name + ".ipt"

                If (IO.File.Exists(newname)) Then
                    Dim ToD As String = TimeOfDay.ToShortTimeString
                    ToD = Replace(ToD, ":", "-")
                    newname = _Filelocation + "\" + _CompDef.SurfaceBodies.Item(i).Name + ToD + ".ipt"
                End If

                NewPrt.SaveAs(newname, False)

                SMCompdef.Unfold()

                Dim DXFName As String = Replace(newname, ".ipt", ".dxf")
                Dim oDataIO As DataIO = NewPrt.ComponentDefinition.DataIO
                oDataIO.WriteDataToFile("FLAT PATTERN DXF?AcadVersion=R12", DXFName)

                NewPrt.Save()
                Dim FFName As String = NewPrt.FullFileName
                NewPrt.Close()

                assy.ComponentDefinition.Occurrences.Add(FFName, oMatrix)
                assy.ComponentDefinition.Occurrences.Item(assy.ComponentDefinition.Occurrences.Count).Grounded = True
                ' _invApp.ScreenUpdating = True
            End If
        Next

        My.Forms.ProgressForm.Close()
        assy.Save()

        Return assy

    End Function

    Private Sub NEST(assy As AssemblyDocument)
        Dim trans As TransientGeometry = _invApp.TransientGeometry
        Dim index As Integer
        Dim Pplatenumber As Integer = _PrimPlates.Count
        Dim Splatenumber As Integer = _SeconPlates.Count
        For Each occ As ComponentOccurrence In assy.ComponentDefinition.Occurrences
            If occ.DefinitionDocumentType = DocumentTypeEnum.kPartDocumentObject Then
                Dim partcomp As PartComponentDefinition = occ.
            End If
            occ.Grounded = False
            index = 0
            If _PrimPlates.Item(index).PlateCutDir = 1 Then
                occ.Definition.
            End If



            Dim firstobj As Object = occ.SurfaceBodies.Item(1).Faces.Item(index) 'occ.SurfaceBodies.Item(1).Faces.Item(1)
            Dim secobj As Object = assy.ComponentDefinition.WorkPlanes.Item(1)
            assy.ComponentDefinition.Constraints.AddFlushConstraint(firstobj, secobj, 0)
        Next

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
