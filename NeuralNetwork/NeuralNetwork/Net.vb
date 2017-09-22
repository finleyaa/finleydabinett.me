Imports System.IO

Public Class Net

    Private InputLayer As New List(Of Neuron)
    Private HiddenLayer As New List(Of Neuron)
    Private OutputLayer As New List(Of Neuron)
    Private Weights As New List(Of Link)
    Private Rnd As New Random
    Public LearningRate As Double

    Public Sub New(ByRef InputFile As String)

        Dim Reader As StreamReader
        Reader = New StreamReader(InputFile)
        Dim Layout As String() = Reader.ReadLine().Split(",")
        LearningRate = Convert.ToDouble(Reader.ReadLine())
        Reader.Close()

        For X = 0 To CInt(Layout(0)) - 1
            InputLayer.Add(New Neuron())
        Next
        For X = 0 To CInt(Layout(1)) - 1
            HiddenLayer.Add(New Neuron())
        Next
        For X = 0 To CInt(Layout(2)) - 1
            OutputLayer.Add(New Neuron())
        Next

        For X = 0 To InputLayer.Count() - 1
            For Y = 0 To HiddenLayer.Count() - 1
                Weights.Add(New Link(InputLayer(X), HiddenLayer(Y), Rnd))
            Next
        Next
        For X = 0 To HiddenLayer.Count() - 1
            For Y = 0 To OutputLayer.Count() - 1
                Weights.Add(New Link(HiddenLayer(X), OutputLayer(Y), Rnd))
            Next
        Next

    End Sub

    Public Sub Train(TrainingData As Object, Iterations As Integer)

        Dim Rnd As New Random
        Dim SelectedValue As New List(Of Double)
        Dim Result As Double
        Dim TrainVals As New List(Of Double)
        Dim Target As Double

        For Num = 1 To Iterations

            TrainVals.Clear()

            SelectedValue = TrainingData(Math.Round(Rnd.Next(TrainingData.Count - 1)))
            TrainVals.AddRange(SelectedValue)
            TrainVals.RemoveAt(TrainVals.Count - 1)
            Target = SelectedValue(SelectedValue.Count - 1)

            Result = Run(TrainVals)
            BackProp(Result, Target)

        Next

    End Sub

    Public Function Test(TestData As Object)

        Dim Results As New List(Of Double)

        For Each Values In TestData
            Results.Add(Run(Values))
        Next

        Return Results

    End Function

    Private Function Run(Values) As Double

        ' Reset all layers
        For X = 0 To InputLayer.Count - 1
            InputLayer(X).SetValue(0)
        Next
        For X = 0 To HiddenLayer.Count - 1
            HiddenLayer(X).SetValue(0)
        Next
        For X = 0 To OutputLayer.Count - 1
            OutputLayer(X).SetValue(0)
        Next

        Dim Result As Double
        For X = 0 To Values.Count - 1
            InputLayer(X).SetValue(Values(X))
        Next

        For X = 0 To (InputLayer.Count) * (HiddenLayer.Count) - 1
            Weights(X).OutputNeuron.AddToValue(Weights(X).InputNeuron.GetValue() * Weights(X).Weight)
        Next
        For X = 0 To HiddenLayer.Count - 1
            Dim NeuronValue As Double = HiddenLayer(X).GetValue()
            HiddenLayer(X).SetValue(Sigmoid(NeuronValue))
        Next
        For X = (InputLayer.Count) * (HiddenLayer.Count) To ((InputLayer.Count) * (HiddenLayer.Count) + (HiddenLayer.Count) * (OutputLayer.Count)) - 1
            Weights(X).OutputNeuron.AddToValue(Weights(X).InputNeuron.GetValue() * Weights(X).Weight)
        Next

        Dim OutputValue As Double = OutputLayer(0).GetValue()

        OutputLayer(0).SetValue(Sigmoid(OutputValue))
        Result = OutputLayer(0).GetValue()

        Return Result

    End Function

    Private Sub BackProp(ActualValue As Double, TargetValue As Double)

        ' Calculate the error (square to make sure it is positive)
        Dim OutputErr As Double = (ActualValue - TargetValue) ^ 2
        ' Calculate the derivative of the output
        Dim DeltaOutput As Double = SigmoidDeriv(OutputLayer(0).GetValue())

        ' Calculate delta of the weights between hidden layer and output layer
        For X = (InputLayer.Count) * (HiddenLayer.Count) To ((InputLayer.Count) * (HiddenLayer.Count) + (HiddenLayer.Count) * (OutputLayer.Count)) - 1
            Weights(X).Delta = OutputErr * DeltaOutput * Weights(X).InputNeuron.GetValue()
        Next

        ' Calculate derr/dnet
        Dim DeltaNetErr As Double = OutputErr * DeltaOutput

        ' Calculate delta hidden error (top to bottom)
        Dim DeltaHiddenErr As New List(Of Double)
        For X = (InputLayer.Count) * (HiddenLayer.Count) To ((InputLayer.Count) * (HiddenLayer.Count) + (HiddenLayer.Count) * (OutputLayer.Count)) - 1
            DeltaHiddenErr.Add(DeltaNetErr * Weights(X).Weight)
        Next

        ' Calculate delta of hidden layer neurons
        For Each Neuron In HiddenLayer
            Neuron.Delta = SigmoidDeriv(Neuron.GetValue())
        Next

        ' Calculate the weight change from input to hidden layer
        Dim IHWeightChange As New List(Of Double)
        For X = 0 To (InputLayer.Count) * (HiddenLayer.Count) - 1
            Dim DeltaHiddenErrValue As Double = DeltaHiddenErr(HiddenLayer.IndexOf(Weights(X).OutputNeuron))
            IHWeightChange.Add(Weights(X).InputNeuron.GetValue() * Weights(X).OutputNeuron.Delta * DeltaHiddenErrValue)
        Next

        ' Update the weights
        For X = 0 To (InputLayer.Count) * (HiddenLayer.Count) - 1
            Weights(X).Weight -= IHWeightChange(X) * LearningRate
        Next
        For X = (InputLayer.Count) * (HiddenLayer.Count) To ((InputLayer.Count) * (HiddenLayer.Count) + (HiddenLayer.Count) * (OutputLayer.Count)) - 1
            Weights(X).Weight -= Weights(X).Delta * LearningRate
        Next

    End Sub

    Private Function Sigmoid(ValToSig As Double) As Double
        Return 1 / (1 + Math.Exp(-ValToSig))
    End Function

    Private Function SigmoidDeriv(ValToSigDeriv As Double) As Double
        Return ValToSigDeriv * (1 - ValToSigDeriv)
    End Function

End Class
