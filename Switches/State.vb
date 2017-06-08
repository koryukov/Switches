Imports System.Runtime.CompilerServices

' ===== Преобразования специфических типов сборки =====
''' <summary>
''' Модуль содержит преобразования специфических типов сборки.
''' </summary>
Public Module Convertor
    ''' <summary>
    ''' Приводит состояние коммутационного аппарата к типу Boolean.
    ''' </summary>
    ''' <param name="state">Состояние коммутационного аппарата <see cref="T:PWDA.Switches.State"/>.</param>
    ''' <returns>Значение <see cref="T:Boolean"/>.
    ''' <example>
    ''' <code language="vb">
    ''' Dim OnState as State = State.I
    ''' Dim OffState as State = State.O
    ''' Dim ans as Boolean
    ''' ans = OnState.ToBoolean     ' True
    ''' ans = OffState.ToBoolean    ' False
    ''' </code>
    ''' </example>
    ''' </returns>
    <Extension> Public Function ToBoolean(ByVal state As State) As Boolean
        Return state.Equals(Switches.State.I)
    End Function
    ''' <summary>
    ''' Конвертирует значение типа Boolean в состояние коммутационного аппарата.
    ''' </summary>
    ''' <param name="bool">Приводимое знечение.</param>
    ''' <returns><see cref="T:PWDA.Switches.State"/>
    ''' <example>
    ''' <code language="vb">
    ''' Dim OnState as State = True.ToState
    ''' Dim OffState as State = False.ToState
    ''' Dim ans as String
    ''' ans = OnState.ToString     ' "on"
    ''' ans = OffState.ToString    ' "off"
    ''' </code>
    ''' </example>
    ''' </returns>
    <Extension> Public Function ToState(ByVal bool As Boolean) As State
        If bool Then
            Return State.I
        Else
            Return State.O
        End If
    End Function
    ''' <summary>
    ''' Конвертирует строку в состояние коммутационного аппарата.
    ''' </summary>
    ''' <param name="str">Строка, конвертируемая в тип <see cref="T:PWDA.Switches.State"/></param>
    ''' <remarks>При неизвестной строке возвращает Nothing.</remarks>
    ''' <returns>
    ''' <list type="bullet">
    '''    <item>
    '''        <term><see cref="F:PWDA.Switches.State.I" /></term>
    '''        <description> from <c>"on"</c>, <c>"true"</c>, <c>"1"</c>, <c>"I"</c>;</description>
    '''    </item>
    '''    <item>
    '''        <term><see cref="F:PWDA.Switches.State.O" /></term>
    '''        <description> from <c>"off"</c>, <c>"false"</c>, <c>"0"</c>, <c>"O"</c>.</description>
    '''    </item>
    ''' </list>
    ''' <example>
    ''' <code language="vb">
    ''' Dim OnState as State = "true".ToState
    ''' Dim OffState as State = "off".ToState
    ''' Dim ans as Boolean
    ''' ans = OnState.Equals(State.I)     ' true
    ''' ans = OffState.Equals(State.O)    ' false
    ''' </code>
    ''' </example>
    ''' </returns>
    <Extension> Public Function ToState(ByVal str As String) As State
        If String.IsNullOrEmpty(str) Then
            Return Nothing
        End If
        Select Case str.ToLower
            Case "on", "true", "i", "1"
                Return State.I
            Case "off", "false", "o", "0"
                Return State.O
            Case Else
                Return Nothing
        End Select
    End Function
End Module


' ===== Состояние выключателя =====
''' <summary>
''' Состояние коммутационного аппарата. Класс обеспечивает возможность объединения контактов с различными схемами включения.
''' </summary>
Public Class State

    Dim _state As Boolean

    ''' <summary>
    ''' Конструктор класса
    ''' </summary>
    Private Sub New(ByVal state As Boolean)
        _state = state
    End Sub

    ''' Members
    ''' <summary>
    ''' Коммутационный аппарат включён.
    ''' </summary>
    Public Shared ReadOnly I As New State(True)
    ''' <summary>
    ''' Коммутационный аппарат выключен.
    ''' </summary>
    Public Shared ReadOnly O As New State(False)

    ' Logic        
    ''' <summary>
    ''' Реализует логическую операцию НЕ (инверсия).
    ''' </summary>
    ''' <param name="state">Состояние коммутационного аппарата.</param>
    ''' <returns>
    ''' Противоположное состояние <see cref="T:PWDA.Switches.State"/> коммутационного аппарата.
    ''' <example>В примере демонстрируется переключение контакта с помощью оператора <c>Not</c>.
    ''' <code language="vb">
    ''' Dim a As State = State.I
    ''' Dim b As State = Not (a)
    ''' Dim ans As String = b.ToString  ' Вернёт "off"
    ''' </code>
    ''' </example>
    ''' </returns>
    Shared Operator Not(ByVal state As State) As State
        Return New State(Not (state._state))
    End Operator
    ''' <summary>
    ''' Реализует логическую операцию И (последовательное включение контактов).
    ''' </summary>
    ''' <param name="a">Состояние первого контакта.</param>
    ''' <param name="b">Состояние второго контакта.</param>
    ''' <returns>
    ''' Состояние <see cref="T:PWDA.Switches.State"/> эквивалентного контакта.
    ''' <example>В примере демонстрируется последовательное включение контактов. Эквивалентный контакт будет замкнут только в том случае, если все контакты в цепи замкнуты.
    ''' <code  language="vb">
    ''' Dim Closed1 As State = State.I
    ''' Dim Closed2 As State = State.I
    ''' Dim Opened1 As State = State.O
    ''' Dim Closed As State = Closed1 And Closed2
    ''' Dim Opened As State = Closed1 And Opened1 And Closed2
    ''' Dim ans As String
    ''' ans = Closed.ToString   ' Вернёт "on"
    ''' ans = Opened.ToString   ' Вернёт "off"
    ''' </code>
    ''' </example>
    ''' </returns>
    Shared Operator And(ByVal a As State, ByVal b As State) As State
        Return New State(a._state And b._state)
    End Operator
    ''' <summary>
    ''' Реализует логическую операцию ИЛИ (параллельное включение контактов).
    ''' </summary>
    ''' <param name="a">Состояние первого контакта.</param>
    ''' <param name="b">Состояние второго контакта.</param>
    ''' <returns>
    ''' Состояние <see cref="T:PWDA.Switches.State"/> эквивалентного контакта.
    ''' <example>В примере демонстрируется параллельное включение контактов. Эквивалентный контакт будет разомкнут только в том случае, если все контакты в цепи разомкнуты.
    ''' <code  language="vb">
    ''' Dim Closed1 As State = State.I
    ''' Dim Opened1 As State = State.O
    ''' Dim Opened2 As State = State.O
    ''' Dim Closed As State = Closed1 Or Opened1 Or Opened2
    ''' Dim Opened As State = Opened1 Or Opened2
    ''' Dim ans As String
    ''' ans = Closed.ToString   ' Вернёт "on"
    ''' ans = Opened.ToString   ' Вернёт "off"
    ''' </code>
    ''' </example>
    ''' </returns>
    Shared Operator Or(ByVal a As State, ByVal b As State) As State
        Return New State(a._state Or b._state)
    End Operator
    ''' <summary>
    ''' Реализует логическую операцию "Исключающее ИЛИ" для двух контактов.
    ''' </summary>
    ''' <param name="a">Состояние первого контакта.</param>
    ''' <param name="b">Состояние второго контакта.</param>
    ''' <returns>
    ''' Состояние <see cref="T:PWDA.Switches.State"/> эквивалентного контакта.
    ''' </returns>
    Shared Operator Xor(ByVal a As State, ByVal b As State) As State
        Return New State(a._state Xor b._state)
    End Operator
    ''' <summary>
    ''' Сравнивает состояния коммутационных аппаратов.
    ''' </summary>
    ''' <param name="a">Состояние первого контакта.</param>
    ''' <param name="b">Состояние второго контакта.</param>
    ''' <returns>
    ''' <list type="bullet">
    '''    <item>
    '''        <term><c>True</c></term>
    '''        <description> в случае равенства.</description>
    '''    </item>
    '''    <item>
    '''        <term><c>False</c></term>
    '''        <description> в случае неравенства.</description>
    '''    </item>
    ''' </list>
    ''' </returns>
    Shared Operator =(ByVal a As State, ByVal b As State) As Boolean
        If ReferenceEquals(a, b) Then
            Return True
        End If
        Return a._state = b._state
    End Operator
    ''' <summary>
    ''' Оператор неравенства.
    ''' </summary>
    ''' <param name="a">Состояние первого контакта.</param>
    ''' <param name="b">Состояние второго контакта.</param>
    ''' <returns>
    ''' <list type="bullet">
    '''    <item>
    '''        <term><c>True</c></term>
    '''        <description> в случае неравенства.</description>
    '''    </item>
    '''    <item>
    '''        <term><c>False</c></term>
    '''        <description> в случае равенства.</description>
    '''    </item>
    ''' </list>
    ''' </returns>
    Shared Operator <>(ByVal a As State, ByVal b As State) As Boolean
        Return a._state <> b._state
    End Operator
    ''' <summary>
    ''' Определяет равентство текущего экземпляра класса ст другим объектом.
    ''' </summary>
    ''' <returns>
    ''' <list type="bullet">
    '''    <item>
    '''        <term>True<c>True</c></term>
    '''        <description> в случае, если объекты равны.</description>
    '''    </item>
    '''    <item>
    '''        <term><c>False</c></term>
    '''        <description> в случае, если объекты неравны.</description>
    '''    </item>
    ''' </list>
    ''' </returns>
    Public Overrides Function Equals(obj As Object) As Boolean
        Dim state As State = TryCast(obj, State)
        If IsNothing(state) Then
            Return False
        Else
            Return Me._state.Equals(state._state)
        End If
    End Function

    ''' <summary>
    ''' Конвертирует состояние коммутационного аппарата в строковое значение.
    ''' </summary>
    ''' <returns>
    ''' <list type="bullet">
    '''    <item>
    '''        <term><c>"on"</c></term>
    '''        <description>включено.</description>
    '''    </item>
    '''    <item>
    '''        <term><c>"off"</c></term>
    '''        <description>выключено/</description>
    '''    </item>
    ''' </list>
    ''' </returns>
    Public Overrides Function ToString() As String
        If _state Then
            Return "on"
        Else
            Return "off"
        End If
    End Function
End Class