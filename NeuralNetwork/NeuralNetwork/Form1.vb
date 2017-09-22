Public Class Form1

    Public NeuralNet As New Net("input.txt")

    Public Sub Form1_Load() Handles MyBase.Load

        Dim TrainData As New List(Of List(Of Double))
        Dim TestData As New List(Of List(Of Double))
        TrainData.Add(New List(Of Double)({2.1, 1.0, 0.5}))
        TestData.Add(New List(Of Double)({2.1, 1.0}))

        NeuralNet.Train(TrainData, 10000)
        Label1.Text = NeuralNet.Test(TestData)(0)

    End Sub

End Class
