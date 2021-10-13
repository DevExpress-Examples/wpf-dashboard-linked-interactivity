Imports DevExpress.DashboardCommon
Imports DevExpress.DashboardCommon.ViewerData
Imports DevExpress.DashboardWpf
Imports System.Collections.Generic
Imports System.Linq
Imports System.Windows

Namespace WpfDashboard_LinkedInteractivity

    ''' <summary>
    ''' Interaction logic for ChildWindow.xaml
    ''' </summary>
    Public Partial Class ChildWindow
        Inherits Window

        Private dControl As DashboardControl

        Public Sub New(ByVal dControl As DashboardControl)
            InitializeComponent()
            Me.dControl = dControl
        End Sub

        Private Sub Window_Loaded(ByVal sender As Object, ByVal e As RoutedEventArgs)
            childDashboardControl.LoadDashboard("Data\DashboardChild.xml")
            Dim dParentControl As DashboardControl = dControl
            dParentControl.MasterFilterSet += AddressOf DControl_MasterFilterSet
            dParentControl.MasterFilterCleared += AddressOf DControl_MasterFilterCleared
            dParentControl.DrillDownPerformed += AddressOf DControl_DrillDownPerformed
            dParentControl.DrillUpPerformed += AddressOf DControl_DrillUpPerformed
        End Sub

        Private Function HasDashboardItem(ByVal itemName As String) As Boolean
            Return childDashboardControl.Dashboard.Items.Select(Function(i) i.ComponentName).Contains(itemName)
        End Function

        Private Sub DControl_MasterFilterSet(ByVal sender As Object, ByVal e As MasterFilterSetEventArgs)
            If HasDashboardItem(e.DashboardItemName) Then
                Dim itemName As String = e.DashboardItemName
                If e.SelectedValues IsNot Nothing Then childDashboardControl.SetMasterFilter(itemName, e.SelectedValues)
                If e.SelectedRange IsNot Nothing Then childDashboardControl.SetRange(itemName, e.SelectedRange)
            End If
        End Sub

        Private Sub DControl_MasterFilterCleared(ByVal sender As Object, ByVal e As MasterFilterClearedEventArgs)
            If HasDashboardItem(e.DashboardItemName) Then
                Dim itemName As String = e.DashboardItemName
                If childDashboardControl.CanClearMasterFilter(itemName) Then childDashboardControl.ClearMasterFilter(itemName)
            End If
        End Sub

        Private Sub DControl_DrillDownPerformed(ByVal sender As Object, ByVal e As DrillActionEventArgs)
            If HasDashboardItem(e.DashboardItemName) Then
                Dim itemName As String = e.DashboardItemName
                Dim row As DashboardDataRow = e.Values(0)
                Dim value As Object = row(row.Length - 1)
                Dim tuple As IList(Of AxisPointTuple) = childDashboardControl.GetAvailableDrillDownValues(itemName)
                Dim availableValues As IEnumerable(Of Object) = tuple.[Select](Function(t) t.GetAxisPoint().UniqueValue)
                If availableValues.Contains(value) Then
                    childDashboardControl.PerformDrillDown(e.DashboardItemName, value)
                End If
            End If
        End Sub

        Private Sub DControl_DrillUpPerformed(ByVal sender As Object, ByVal e As DrillActionEventArgs)
            If HasDashboardItem(e.DashboardItemName) Then
                Dim itemName As String = e.DashboardItemName
                Dim level As Integer = e.DrillDownLevel
                Dim tuple As AxisPointTuple = childDashboardControl.GetCurrentDrillDownValues(itemName)
                If tuple IsNot Nothing Then
                    Dim point As AxisPoint = childDashboardControl.GetCurrentDrillDownValues(itemName).GetAxisPoint()
                    Dim l As Integer = 0
                    While point.Parent IsNot Nothing
                        l += 1
                        point = point.Parent
                    End While

                    If level + 1 = l AndAlso childDashboardControl.CanPerformDrillUp(itemName) Then childDashboardControl.PerformDrillUp(itemName)
                End If
            End If
        End Sub
    End Class
End Namespace
