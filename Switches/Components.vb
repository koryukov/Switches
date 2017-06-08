' ===== Интерфейс переключаемого устройства =====
''' <summary>
''' Определяет набор свойств и процедур, реализуемых коммутационным аппаратом, который может находиться в двух состояниях.
''' </summary>
Public Interface ISwitchable
    ''' <summary>
    ''' Текущее состояние переключаемого устройства
    ''' </summary>
    ''' <value><see cref="T:PWDA.Switches.State"/></value>
    Property State As State
    ''' <summary>
    ''' Возвращает <c>True</c>, если устройство включено, в противном случае - <c>False</c>
    ''' </summary>
    ''' <remarks>Свойство доступно только для чтения.</remarks>
    ''' <value>Boolean</value>
    ReadOnly Property IsON As Boolean
    ''' <summary>
    ''' Возвращает True, если устройство выключено, в противном случае - False
    ''' </summary>
    ''' <remarks>Свойство доступно только для чтения.</remarks>
    ''' <value>Boolean</value>
    ReadOnly Property IsOFF As Boolean
    ''' <summary>
    ''' Переключает устройство из одного состояния в другое.
    ''' </summary>
    Sub Switch()
    ''' <summary>
    ''' Переключает изделие в состояние, заданное передаваемым аргументом state.
    ''' </summary>
    ''' <param name="state">Новое состояние коммутационного аппарата <see cref="T:PWDA.Switches.State"/>.</param>
    Sub SwitchTo(ByVal state As State)
    ''' <summary>
    ''' Переводит устройство в состояние ВКЛЮЧЕНО
    ''' </summary>
    Sub SwitchON()
    ''' <summary>
    ''' Переводит устройство в состояние ВЫКЛЮЧЕНО
    ''' </summary>
    Sub SwitchOFF()
End Interface

' ===== Переключаемое устройство с двумя состояниями =====
' Используется для создания коммутациоонных аппаратов
''' <summary>
''' Абстрактный класс простого коммутационого аппарата, который может находиться в двух состояниях.
''' </summary>
''' <remarks>
''' Класс предназначен для создания коммутационных аппаратов. Создание экземпляров этого класса невозможно.
''' </remarks>
Public MustInherit Class SimpleSwitch
    Implements ISwitchable
    ' --- События ---
    ''' <summary>
    ''' Событие, возникающее перед установкой состояния устройства.
    ''' </summary>
    ''' <param name="oldState">Текущее состояние коммутационного аппарата.</param>
    ''' <param name="newState">Новое состояние коммутационного аппарата. Доступно для редактирования.</param>
    ''' <remarks>
    ''' Доступно в наследумых классах.
    ''' Передаёт обработчику старое и новое состояние устройства с возможностью изменить новое состояние в обработчике.
    ''' </remarks>
    Protected Event BeforeSetState(ByVal oldState As State, ByRef newState As State)
    ''' <summary>
    ''' Событие, возникающее после установки состояния устройства.
    ''' </summary>
    ''' <param name="oldState">Текущее состояние коммутационного аппарата.</param>
    ''' <param name="newState">Новое состояние коммутационного аппарата. Доступно для редактирования.</param>
    ''' <remarks>
    ''' Доступно в наследумых классах.
    ''' Передаёт обработчику старое и новое состояние устройства с возможностью изменить новое состояние в обработчике.
    ''' </remarks>
    Protected Event AfterSetState(ByVal oldState As State, ByRef newState As State)
    ''' <summary>
    ''' Событие, возникающее перед получением текущего состояния устройства.
    ''' </summary>
    ''' <remarks>Доступно в наследумых классах. Не имеет аргументов.</remarks>
    Protected Event BeforeGetState()
    ''' <summary>
    ''' Событие, возникающее после получения текущего состояния устройства.
    ''' </summary>
    ''' <remarks>Доступно в наследумых классах. Не имеет аргументов.</remarks>
    Protected Event AfterGetState()

    ''' <summary>
    ''' Подпись переключаемого устройства.
    ''' </summary>
    ''' <remarks>Доступно только членам текущего экземпляра класса.</remarks>
    ''' Локальные переменные
    Dim _label As String
    ''' <summary>
    ''' Текущее состояние переключаемого устройства
    ''' </summary>
    ''' <remarks>Доступно только членам текущего экземпляра класса.</remarks>
    Dim _state As State

    ' --- Конструктор ---
    ''' <summary>
    ''' Конструктор класса по умолчанию. Создаёт коммутационный аппарат в выключенном состоянии.
    ''' </summary>
    Public Sub New()
        _state = Switches.State.O
    End Sub
    ''' <summary>
    ''' Создаёт простое переключаемое устройство с заданным начальным состоянием.
    ''' </summary>
    ''' <param name="state">Заданное начальное состояние переключаемого устройства.</param>
    Public Sub New(ByVal state As State)
        _state = state
    End Sub
    ''' <summary>
    ''' Создаёт простое переключаемое устройство в выключенном состоянии с подписью.
    ''' </summary>
    ''' <param name="label">Подпись устройства.</param>
    Public Sub New(ByVal label As String)
        Me.New()
        Me.Label = label
    End Sub
    ''' <summary>
    ''' Создаёт простое переключаемое устройство с подписью и заданным начальным состоянием.
    ''' </summary>
    ''' <param name="label">Подпись устройства.</param>
    ''' <param name="state">Заданное начальное состояние переключаемого устройства.</param>
    Public Sub New(ByVal label As String, ByVal state As State)
        Me.New(state)
        Me.Label = label
    End Sub

    '--- Метка ---
    ''' <summary>
    ''' Подпись переключаемого устройства
    ''' </summary>
    ''' <value>String</value>
    Public Property Label As String
        Get
            Return _label
        End Get
        Set(value As String)
            _label = value
        End Set
    End Property

    ' --- Состояние ---
    ''' <summary>
    ''' Текущее состояние переключаемого устройства.
    ''' </summary>
    ''' <value><see cref="T:PWDA.Switches.State"/></value>
    Public Property State As State Implements ISwitchable.State
        Get
            RaiseEvent BeforeGetState()
            Return _state
            RaiseEvent AfterGetState()
        End Get
        Set(value As State)
            Dim old As State = _state
            RaiseEvent BeforeSetState(old, value)
            If IsNothing(State) Then
                Throw New ArgumentNullException("state", "Методу передан пустой эксземпляр объекта")
            End If
            _state = value
            RaiseEvent AfterSetState(old, value)
        End Set
    End Property

    ''' <summary>
    ''' Переключает коммутационный аппарат из одного состояния в другое.
    ''' </summary>
    Public Sub Switch() Implements ISwitchable.Switch
        Me.State = Not (_state)
    End Sub
    ''' <summary>
    ''' Переключает коммутационный аппрат в заданное состояние.
    ''' </summary>
    ''' <param name="state">Состояние, в которое будет перведён коммутационный аппарат.</param>
    Public Sub SwitchTo(ByVal state As State) Implements ISwitchable.SwitchTo
        Me.State = state
    End Sub
    ''' <summary>
    ''' Включает коммутационный аппарат.
    ''' </summary>
    Public Sub SwitchON() Implements ISwitchable.SwitchON
        Me.State = Switches.State.I
    End Sub
    ''' <summary>
    ''' Выключает коммутационный аппарат.
    ''' </summary>
    Public Sub SwitchOFF() Implements ISwitchable.SwitchOFF
        Me.State = Switches.State.O
    End Sub

    ''' <summary>
    ''' Возвращает <c>True</c>, если устройство включенов противном случае - <c>False</c>.
    ''' </summary>
    ''' <remarks>Свойство доступно только для чтения.</remarks>
    ''' <value>Boolean</value>
    Public ReadOnly Property IsON As Boolean Implements ISwitchable.IsON
        Get
            Return Me.State.Equals(Switches.State.I)
        End Get
    End Property
    ''' <summary>
    ''' Возвращает <c>True</c>, если устройство выключено, в противном случае - <c>False</c>.
    ''' </summary>
    ''' <remarks>Свойство доступно только для чтения.</remarks>
    ''' <value>Boolean</value>
    Public ReadOnly Property IsOFF As Boolean Implements ISwitchable.IsOFF
        Get
            Return Me.State.Equals(Switches.State.O)
        End Get
    End Property
End Class

' ===== Обмотка реле =====
''' <summary>
''' Воспринимающая часть коммутационного аппарата
''' </summary>
Public Class Coil
    Inherits SimpleSwitch

    ''' <summary>
    ''' Конструктор класса по умолчанию. Создаёт воспринимающую часть коммутационного аппарата в выключенном состоянии.
    ''' </summary>
    Public Sub New()
        MyBase.New()
    End Sub
    ''' <summary>
    ''' Создаёт воспринимающую часть коммутационного аппарата с заданным начальным состоянием.
    ''' </summary>
    ''' <param name="state">Задаёт начальное сосотояние катушки (см. свойство <see cref="P:PWDA.Switches.SimpleSwitch.State"/>).</param>
    Public Sub New(ByVal state As State)
        MyBase.New(state)
    End Sub
    ''' <summary>
    ''' Создаёт воспринимающую часть коммутационного аппарата с подписью в выключенном состоянии.
    ''' </summary>
    ''' <param name="label">Подпись к катушке (см. свойство <see cref="P:PWDA.Switches.SimpleSwitch.Label"/>).</param>
    Public Sub New(ByVal label As String)
        MyBase.New(label)
    End Sub
    ''' <summary>
    ''' Создаёт воспринимающую часть коммутационного аппарата с заданным начальным состоянием и подписью
    ''' </summary>
    ''' <param name="label">Подпись к катушке (см. свойство <see cref="P:PWDA.Switches.SimpleSwitch.Label"/>).</param>
    ''' <param name="state">Задаёт начальное сосотояние катушки (см. свойство <see cref="P:PWDA.Switches.SimpleSwitch.State"/>).</param>
    Public Sub New(ByVal label As String, ByVal state As State)
        MyBase.New(label, state)
    End Sub
End Class

' ===== Простой однополюсный контакт =====
''' <summary>
''' Простой однополюсный электрический контакт
''' </summary>
Public Class Contact
    Inherits SimpleSwitch

    ''' <summary>
    ''' Конструктор класса по умолчанию. Создаёт нормально разомкнутый контакт.
    ''' </summary>
    Public Sub New()
        MyBase.New()
    End Sub
    ''' <summary>
    ''' Создаёт контакт с заданным начальным состоянием.
    ''' </summary>
    ''' <param name="state">Начальное состояние контакта (см. свойство <see cref="P:PWDA.Switches.SimpleSwitch.State"/>).</param>
    Public Sub New(ByVal state As State)
        MyBase.New(state)
    End Sub
    ''' <summary>
    ''' Создаёт нормально разомкрутый контакт с заданной подписью.
    ''' </summary>
    ''' <param name="label">Подпись к контакту (см. свойство <see cref="P:PWDA.Switches.SimpleSwitch.Label"/>).</param>
    Public Sub New(ByVal label As String)
        MyBase.New(label)
    End Sub
    ''' <summary>
    ''' Создаёт контакт с заданным начальным состоянием и подписью.
    ''' </summary>
    ''' <param name="label">Подпись к контакту (см. свойство <see cref="P:PWDA.Switches.SimpleSwitch.Label"/>).</param>
    ''' <param name="state">Начальное состояние контакта (см. свойство <see cref="P:PWDA.Switches.SimpleSwitch.State"/>).</param>
    Public Sub New(ByVal label As String, ByVal state As State)
        MyBase.New(label, state)
    End Sub
End Class