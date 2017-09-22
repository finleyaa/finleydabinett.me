Public Class Neuron

    Private Value As Double
    Public Delta As Double

    Public Sub SetValue(NewValue As Double)
        Value = NewValue
    End Sub

    Public Function GetValue() As Double
        Return Value
    End Function

    Public Sub AddToValue(AddValue As Double)
        Value += AddValue
    End Sub

End Class
