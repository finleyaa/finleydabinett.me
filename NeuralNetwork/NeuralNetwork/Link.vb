Public Class Link

    Public Weight As Double
    Public InputNeuron As Neuron
    Public OutputNeuron As Neuron
    Public Delta As Double

    Public Sub New(ByRef Input As Neuron, ByRef Output As Neuron, Rnd As Random)

        InputNeuron = Input
        OutputNeuron = Output
        Weight = Rnd.Next(-100, 101) / 100

    End Sub

End Class
