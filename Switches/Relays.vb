Imports System.Net
Imports System.Configuration
Imports System.Text.RegularExpressions
Imports System.Runtime.CompilerServices

<Assembly: InternalsVisibleTo("SwitchesUnitTests")> 

' ===== Коммутационный аппарат типа РЕЛЕ =====
''' <summary>
''' Коммутационный аппарат дистанционного управления
''' </summary>
Public Class Relay
    Inherits Coil

    ''' <summary>
    ''' Исполнительная часть коммутационного аппарата (группа контактов).
    ''' </summary>
    ''' <remarks>Доступно только членам текущего экземпляря класса.</remarks>
    Dim _contacts As List(Of Contact)

    ''' <summary>
    ''' Событие, возникающее перед получением текущего состояния исполнительной части коммутационного аппарата. Предназначено для использования в производных классах.
    ''' </summary>
    ''' События
    Event BeforeGetContact()

    ''' <summary>
    ''' Событие, возникающее после получения текущего состояния исполнительной части коммутационного аппарата. Предназначено для использования в производных классах.
    ''' </summary>
    Event AfterGetContact()

    ''' <summary>
    ''' Конструктор класса по умолчанию. Создаёт выключенный коммутационный аппарат с одним нормально разомкнутым контактом.
    ''' </summary>
    Public Sub New()
        Me.New(String.Empty, Switches.State.O)
    End Sub
    ''' <summary>
    ''' Создаёт коммутационный аппарат с одним нормально разомкнутым контактом и переводит его в заданное состояние.
    ''' </summary>
    ''' <param name="state">Состояние коммутационного аппарата.</param>
    Public Sub New(ByVal state As State)
        Me.New(String.Empty, state)
    End Sub
    ''' <summary>
    ''' Создаёт выключенный коммутационный аппарат с подписью с одним нормально разомкнутым контактом.
    ''' </summary>
    ''' <param name="label">Подпись коммутационного аппарата (см. свойство <see cref="P:PWDA.Switches.SimpleSwitch.Label"/>).</param>
    Public Sub New(ByVal label As String)
        Me.New(label, Switches.State.O)
    End Sub
    ''' <summary>
    ''' Создаёт коммутационный аппарат с подписью с одним нормально разомкнутым контактом и переводит его в заданное состояние.
    ''' </summary>
    ''' <param name="label">Подпись коммутационного аппарата (см. свойство <see cref="P:PWDA.Switches.SimpleSwitch.Label"/>).</param>
    ''' <param name="state">Состояние коммутационного аппарата (см. свойство <see cref="P:PWDA.Switches.SimpleSwitch.State"/>).</param>
    Public Sub New(ByVal label As String, ByVal state As State)
        Me.New(label, state, New List(Of Contact)({New Contact}))
    End Sub
    ''' <summary>
    ''' Создаёт коммутационный аппарат с подписью с одним контактом, определённым пользователем, и переводит его в заданное состояние.
    ''' </summary>
    ''' <param name="label">Подпись коммутационного аппарата (см. свойство <see cref="P:PWDA.Switches.SimpleSwitch.Label"/>).</param>
    ''' <param name="state">Состояние коммутационного аппарата (см. свойство <see cref="P:PWDA.Switches.SimpleSwitch.State"/>).</param>
    ''' <param name="contact">Управляемый контакт.</param>
    Public Sub New(ByVal label As String, ByVal state As State, contact As Contact)
        Me.New(label, state, New List(Of Contact)({contact}))
    End Sub
    ''' <summary>
    ''' Создаёт коммутационный аппарат с подписью с группой контактов, определённой пользователем, и переводит его в заданное состояние.
    ''' </summary>
    ''' <param name="label">Подпись коммутационного аппарата (см. свойство <see cref="P:PWDA.Switches.SimpleSwitch.Label"/>).</param>
    ''' <param name="state">Состояние коммутационного аппарата (см. свойство <see cref="P:PWDA.Switches.SimpleSwitch.State"/>).</param>
    ''' <param name="contacts">Управляемая группа контактов (см. свойство <see cref="P:PWDA.Switches.Relay.Contacts"/>).</param>
    Public Sub New(ByVal label As String, ByVal state As State, contacts As List(Of Contact))
        MyBase.New(label, state)
        _contacts = New List(Of Contact)(contacts)
    End Sub

    ''' <summary>
    ''' Исполнительная часть коммутационного аппарата (группа контактов).
    ''' </summary>
    ''' <remarks>Доступно только для чтения.</remarks>
    ''' <value>Перечисляемый список контактов коммутационного аппарата.</value>
    Public ReadOnly Property Contacts As List(Of Contact)
        Get
            RaiseEvent BeforeGetContact()
            Return _contacts
            RaiseEvent AfterGetContact()
        End Get
    End Property

    ''' <summary>
    ''' Обеспечивает переключение управляемых контактов при переключении коммутационного аппарата.
    ''' </summary>
    ''' <param name="oldState">Текущее состояние коммутационного аппарата.</param>
    ''' <param name="newState">Новое состояние коммутационного аппарата. Доступно для редактирования.</param>
    ''' <remarks>Доступно членам текущего экземпляра класса.</remarks>
    Private Sub Switch_Handler(ByVal oldState As State, ByRef newState As State) Handles Me.AfterSetState
        If oldState <> newState Then
            Me._contacts.ForEach(Sub(contact As Contact) contact.Switch())
        End If
    End Sub

    ''' <summary>
    ''' Получает контакт по его подписи.
    ''' </summary>
    ''' <remarks>Если коммутационный аппарат имеет несколько контактов с одинаковой подписью, возвращается только первый из них.</remarks>
    ''' Выборка контактов
    ''' <param name="label">Искомая подпись контакта.</param>
    ''' <returns>Искомый контакт.</returns>
    Public Function GetContact(ByVal label As String) As Contact
        Return Me.Contacts.Find(Function(c As Contact) String.Equals(c.Label, label))
    End Function
    ''' <summary>
    ''' Получает контакт по его состоянию.
    ''' </summary>
    ''' <remarks>Если коммутационный аппарат имеет несколько контактов, находящихся в одинаковом состоянии, возвращается только первый из них.</remarks>
    ''' <param name="state">Искомое состояние контакта.</param>
    Public Function GetContact(ByVal state As State) As Contact
        Return Me.Contacts.Find(Function(c As Contact) c.State Is state)
    End Function
    ''' <summary>
    ''' Возвращает список контактов с заданной подписью
    ''' </summary>
    ''' <param name="label">Искомая подпись контакта.</param>
    ''' <returns>List(Of Contacts)</returns>
    Public Function GetContacts(ByVal label As String) As List(Of Contact)
        Return Me.Contacts.FindAll(Function(c As Contact) String.Equals(c.Label, label))
    End Function
    ''' <summary>
    ''' Возвращает список контактов, находящихся в заданном состоянии.
    ''' </summary>
    ''' <returns>Перечисляемый список контактов.</returns>
    Public Function GetContacts(ByVal state As State) As List(Of Contact)
        Return Me.Contacts.FindAll(Function(c As Contact) c.State Is state)
    End Function
End Class

' ===== Сетевое реле ====
''' <inheritdoc />
''' <summary>
''' Коммутационный аппарат, управляемый сетевыми сообщениями.
''' </summary>
Public Class NetworkRelay
    Inherits Relay

    ''' <summary>
    ''' Строка подключения к удалённому коммутационному аппарату.
    ''' </summary>
    ''' <remarks>Доступен только членам внутри текущего экземпляра класса.</remarks>
    Dim _connection As Connection
    ''' <summary>
    ''' Автоматическая синхноизация с удалённым устройством.
    ''' </summary>
    ''' <remarks>Доступен только членам внутри текущего экземпляра класса.</remarks>
    Dim _autosync As Boolean

    ''' <summary>
    ''' Создаёт выключенный дистанционный коммутационный аппарат с заданной строкой подключения и одним нормально разомкнутым контактом.
    ''' </summary>
    ''' <param name="ConnectionString">
    ''' Строка подключения к удалённому коммутационному аппарату (см. свойство <see cref="P:PWDA.Switches.NetworkRelay.ConnectionString"/>).
    ''' </param>
    ''' <param name="AutoSync">Автоматическая синхноизация с удалённым устройством (см. свойство <see cref="P:PWDA.Switches.NetworkRelay.AutoSync"/>). По умолчанию равен <c>False</c>.</param>
    ''' <exception cref="T:System.InvalidOperationException">Возникает при установке параметру <paramref name="AutoSync"/> значения <c>True</c> в случае ошибки при получении данных от удалённого коммутационного аппарата.</exception>
    Public Sub New(ByVal ConnectionString As String, Optional ByVal AutoSync As Boolean = False)
        Me.New(String.Empty, ConnectionString, New List(Of Contact)({New Contact()}), AutoSync)
    End Sub

    ''' <summary>
    ''' Создаёт выключенный дистанционный коммутационный аппарат с заданной строкой подключения, подписью и одним нормально разомкнутым контактом.
    ''' </summary>
    ''' <param name="label">Подпись коммутационного аппарата (см. свойство <see cref="P:PWDA.Switches.SimpleSwitch.Label"/>).</param>
    ''' <param name="ConnectionString">Строка подключения к удалённому коммутационному аппарату (см. свойство <see cref="P:PWDA.Switches.NetworkRelay.ConnectionString"/>).</param>
    ''' <param name="AutoSync">Автоматическая синхноизация с удалённым устройством (см. свойство <see cref="P:PWDA.Switches.NetworkRelay.AutoSync"/>). По умолчанию равен <c>False</c>.</param>
    ''' <exception cref="T:System.InvalidOperationException">Возникает при установке параметру <paramref name="AutoSync"/> значения <c>True</c> в случае ошибки при получении данных от удалённого коммутационного аппарата.</exception>
    Public Sub New(ByVal label As String, ByVal ConnectionString As String, Optional ByVal AutoSync As Boolean = False)
        Me.New(label, ConnectionString, New List(Of Contact)({New Contact()}), AutoSync)
    End Sub
    ''' <summary>
    ''' Создаёт выключенный дистанционный коммутационный аппарат с заданной строкой подключения, подписью и одним контактом, заданным пользователем.
    ''' </summary>
    ''' <param name="label">Подпись коммутационного аппарата (см. свойство <see cref="P:PWDA.Switches.SimpleSwitch.Label"/>).</param>
    ''' <param name="ConnectionString">Строка подключения к удалённому коммутационному аппарату (см. свойство <see cref="P:PWDA.Switches.NetworkRelay.ConnectionString"/>).</param>
    ''' <param name="contact">Контакт исполнительной части (см. свойство <see cref="P:PWDA.Switches.Relay.Contacts"/>)</param>
    ''' <param name="AutoSync">Автоматическая синхноизация с удалённым устройством (см. свойство <see cref="P:PWDA.Switches.NetworkRelay.AutoSync"/>). По умолчанию равен <c>False</c>.</param>
    ''' <exception cref="T:System.InvalidOperationException">Возникает при установке параметру <paramref name="AutoSync"/> значения <c>True</c> в случае ошибки при получении данных от удалённого коммутационного аппарата.</exception>
    Public Sub New(ByVal label As String, ByVal ConnectionString As String, contact As Contact, Optional ByVal AutoSync As Boolean = False)
        Me.New(label, ConnectionString, New List(Of Contact)({contact}), AutoSync)
    End Sub
    ''' <summary>
    ''' Создаёт выключенный дистанционный коммутационный аппарат с заданной строкой подключения, подписью и исполнительной частью, определённой пользователем.
    ''' </summary>
    ''' <param name="label">Подпись коммутационного аппарата (см. свойство <see cref="P:PWDA.Switches.SimpleSwitch.Label"/>)</param>
    ''' <param name="ConnectionString">Строка подключения к удалённому коммутационному аппарату (см. свойство <see cref="P:PWDA.Switches.NetworkRelay.ConnectionString"/>).</param>
    ''' <param name="contacts">Исполнительная часть (группа контактов). См. свойство <see cref="P:PWDA.Switches.Relay.Contacts"/></param>
    ''' <param name="AutoSync">Автоматическая синхноизация с удалённым устройством (см. свойство <see cref="P:PWDA.Switches.NetworkRelay.AutoSync"/>). По умолчанию равен <c>False</c>.</param>
    ''' <exception cref="T:System.InvalidOperationException">Возникает при установке параметру <paramref name="AutoSync"/> значения <c>True</c> в случае ошибки при получении данных от удалённого коммутационного аппарата.</exception>
    Public Sub New(ByVal label As String, ByVal ConnectionString As String, contacts As List(Of Contact), Optional ByVal AutoSync As Boolean = False)
        MyBase.New(label, Switches.State.O, contacts)
        _connection = New Connection(ConnectionString)
        Me.AutoSync = AutoSync
    End Sub

    ''' <summary>
    ''' Строка подключения к удалённому коммутационному аппарату.
    ''' </summary>
    ''' <value>String</value>
    ''' <exception cref="T:System.ArgumentException">Возникает в случае передачи неверной строки подключения.</exception>
    Public Property ConnectionString As String
        Get
            Return _connection.ConnectionString
        End Get
        Set(value As String)
            _connection.ConnectionString = value
        End Set
    End Property

    ''' <summary>
    ''' Автоматическая синхноизация с удалённым устройством.
    ''' </summary>
    ''' <remarks>При включении автоматической синхронизации происходит автоматическая загрузка состояния исполнительной части с удалённого коммутационного аппарата.</remarks>
    ''' <value>Boolean</value>
    ''' <exception cref="T:System.InvalidOperationException">Возникает при установке данному свойству значения <c>True</c> в случае ошибки при получении данных от удалённого коммутационного аппарата.</exception>
    Public Property AutoSync As Boolean
        Get
            Return _autosync
        End Get
        Set(value As Boolean)
            _autosync = value
            If _autosync Then
                Call Me.Download()
            End If
        End Set
    End Property

    ''' <summary>
    ''' Загружает текущее состояние исполнительной части коммутационного аппарата из удалённого устройства.
    ''' </summary>
    ''' <exception cref="T:System.InvalidOperationException">Возникает в случае ошибки при получении данных от удалённого коммутационного аппарата.</exception>
    Public Sub Download()
        Dim oldAutoSync As Boolean = _autosync
        _autosync = False
        Try
            Me.State = _connection.Read
        Catch ex As Exception
            Throw New InvalidOperationException("Ошибка загрузки данных сетевого реле", ex)
        Finally
            _autosync = oldAutoSync
        End Try
    End Sub
    ''' <summary>
    ''' Переключает удалённый коммутационный аппарат, заданный новой строкой подключения, в состояние текущего экземпляра класса.
    ''' </summary>
    ''' <remarks>Метод изменяет строку подключения.</remarks>
    ''' <param name="ConnectionString">Новая строка подключения к удалённому коммутационному аппапату (см. свойство <see cref="P:PWDA.Switches.NetworkRelay.ConnectionString"/>).</param>
    ''' <exception cref="T:System.InvalidOperationException">Возникает в случае ошибки при получении данных от удалённого коммутационного аппарата.</exception>
    Public Sub Download(ByVal ConnectionString As String)
        Me.ConnectionString = ConnectionString
        Call Download()
    End Sub

    ''' <summary>
    ''' Переключает удалённый коммутационный аппарат в состояние текущего экземпляра класса.
    ''' </summary>
    ''' <exception cref="T:System.InvalidOperationException">Возникает в случае ошибки при передаче данных удалённому коммутационному аппарату.</exception>
    Public Sub Upload()
        Dim oldAutoSync As Boolean = _autosync
        _autosync = False
        Try
            Me._connection.Write(Me.State)
        Catch ex As Exception
            Throw New InvalidOperationException("Ошибка передачи данных сетевому реле", ex)
        Finally
            _autosync = oldAutoSync
        End Try
    End Sub
    ''' <summary>
    ''' Переключает удалённый коммутационный аппарат, заданный новой строкой подключения, в состояние текущего экземпляра класса.
    ''' </summary>
    ''' <remarks>Метод изменяет строку подключения.</remarks>
    ''' <param name="ConnectionString">Новая строка подключения к удалённому коммутационному аппапату (см. свойство <see cref="P:PWDA.Switches.NetworkRelay.ConnectionString"/>).</param>
    ''' <exception cref="T:System.InvalidOperationException">Возникает в случае ошибки при передаче данных удалённому коммутационному аппарату.</exception>
    Public Sub Upload(ByVal ConnectionString As String)
        Me.ConnectionString = ConnectionString
        Call Upload()
    End Sub

    ''' <summary>
    ''' Автоматическая синхронизация текущего экземпляра класса с удалённым коммутационным аппаратом при запросе текущего состояния исполнительной или воспринимающей части аппарата. 
    ''' Обработчик активен при включённой автоматической синхронизации.
    ''' </summary>
    ''' <remarks>Доступен членам текущего экземпляра класса.</remarks>
    ''' <exception cref="T:System.InvalidOperationException">Возникает в случае ошибки при получении данных от удалённого коммутационного аппарата.</exception>
    Private Sub GetState_Handler() Handles Me.BeforeGetContact, Me.BeforeGetState
        If _autosync Then
            _autosync = False
            Call Me.Download()
            _autosync = True
        End If
    End Sub
    ''' <summary>
    ''' Автоматическая синхронизация текущего экземпляра класса с удалённым коммутационным аппаратом при изменении текущего состояния воспринимающей части аппарата. 
    ''' Обработчик активен при включённой автоматической синхронизации.
    ''' </summary>
    ''' <remarks>Доступен членам текущего экземпляра класса.</remarks>
    ''' <exception cref="T:System.InvalidOperationException">Возникает в случае ошибки при передаче данных удалённому коммутационному аппарату.</exception>
    Private Sub SetState_Handler() Handles Me.AfterSetState
        If _autosync Then
            _autosync = False
            Call Me.Upload()
            _autosync = True
        End If
    End Sub
End Class

' ===== Подключение к сетевому переключателю =====

''' <summary>
''' Подключение к удалённому коммутационному аппарату, управляемому сетевыми сообщениями.
''' </summary>
''' <remarks>Доступен из текущей сборки.</remarks>
Friend Class Connection
    ''' <summary>
    ''' Строка подключения к удалённому коммутационному аппарату.
    ''' </summary>
    ''' <remarks>Доступно только членам текущего экземпляра класса.</remarks>
    Dim _connString As String

    ' Сегменты строки подключения

    ''' <summary>
    ''' Перечисление имён фрагметов строки подключения
    ''' </summary>
    ''' <remarks>Доступен членам текущего экземпляра класса и в наследуемых классах.</remarks>
    Protected Enum Parts
        ServiceURL
        OnCommand
        OffCommand
        GetCommand
    End Enum
    ''' <summary>
    ''' Перечень фрагментов строки подключения и соответствующих им шаблонов регулярных выражений.
    ''' </summary>
    ''' <remarks>Доступно только членам текущего экземпляра класса.</remarks>
    Dim _templates As New Dictionary(Of Parts, String) From {
            {Parts.ServiceURL, "(?<=ServiceURL\s?=\s?)[^;\s\t\n]+"}, _
            {Parts.OnCommand, "(?<=OnCommand\s?=\s?/?)[^;\s\t\n/][^;\s\t\n]*"}, _
            {Parts.OffCommand, "(?<=OffCommand\s?=\s?/?)[^;\s\t\n/][^;\s\t\n]*"}, _
            {Parts.GetCommand, "(?<=GetCommand\s?=\s?/?)[^;\s\t\n/][^;\s\t\n]*"} _
            }
    ''' <summary>
    ''' Перечень фрагментов строки подключения и соответствующих им URL.
    ''' </summary>
    ''' <remarks>Доступно только членам текущего экземпляра класса.</remarks>
    Dim _connParts As New Dictionary(Of Parts, Uri)(_templates.Count)

    ''' <summary>
    ''' Конструктор класса.
    ''' </summary>
    ''' Конструктор
    ''' <param name="ConnectionString">Строка подключения к управляемому по сети коммутационному аппарату <see cref="P:PWDA.Switches.Connecion.ConnectionString"/>.</param>
    ''' <exception cref="T:System.ArgumentException">Возникает в случае передачи методу неверной строки подключения.</exception>
    Sub New(ByVal ConnectionString As String)
        If ConnectionString.Contains(";") Then
            Me.ConnectionString = ConnectionString
        Else
            Me.ConnectionString = ConfigurationManager.ConnectionStrings(ConnectionString).ConnectionString
        End If
    End Sub

    ''' <summary>
    ''' Строка подключения к удалённому коммутационному аппарату.
    ''' </summary>
    ''' <value>String</value>
    ''' Строка подключения
    ''' <exception cref="T:System.ArgumentException">Возникает в случае передачи неверной строки подключения.</exception>
    Public Property ConnectionString As String
        Get
            Return _connString
        End Get
        Set(value As String)
            Try
                _connParts = _templates.ToDictionary( _
                                    Function(pair As KeyValuePair(Of Parts, String)) pair.Key, _
                                    Function(pair As KeyValuePair(Of Parts, String)) CreateUri(value, pair.Key))
            Catch ex As UriFormatException
                Throw New ArgumentException("Ошибка при передаче методу значения параметра.", "ConnectionString", ex)
            End Try
            _connString = value
        End Set
    End Property
    ''' <summary>
    ''' Формирует URL запроса для указанной команды.
    ''' </summary>
    ''' <param name="connstring">Строка подключения к удалённому коммутационному аппарату <see cref="P:PWDA.Switches.Connecion.ConnectionString"/>.</param>
    ''' <param name="part">Идентификатор фрагмента строки подключения (см. <see cref="T:PWDA.Switches.Connection.Parts"/>).</param>
    ''' <returns><see cref="T:Sysyem.Uri"/></returns>
    ''' <exception cref="T:System.UriFormatException">Строка подключения к службе управления внешними устройствами имеет неверный формат.</exception>
    Private Function CreateUri(ByVal connstring As String, ByVal part As Parts) As Uri
        Dim url As String = Regex.Match(connstring, _templates(part), RegexOptions.IgnoreCase).Value
        If String.IsNullOrEmpty(url) Then
            Throw New UriFormatException("Строка подключения к службе управления внешними устройствами имеет неверный формат. Сегмент " + part.ToString + " указан неверно или отсутствует.")
        End If
        Dim kind As UriKind
        If part = Parts.ServiceURL Then
            kind = UriKind.Absolute
            If Not url.EndsWith("/") Then
                url = url + "/"
            End If
        Else
            kind = UriKind.Relative
        End If
        Return New Uri(url, kind)
    End Function

    ''' <summary>
    ''' Перечень фрагментов строки подключения и соответствующих им URL.
    ''' </summary>
    ''' <remarks>Доступно в наследуемых классах.</remarks>
    ''' <value>Dictionary(Of Parts, Uri)</value>
    Protected ReadOnly Property ConnectionParts As Dictionary(Of Parts, Uri)
        Get
            Return _connParts
        End Get
    End Property

    ''' <summary>
    ''' Подключается к удалённому коммутационному аппарату и устанавливает его состояние, заданное передаваемым агрументом.
    ''' </summary>
    ''' <param name="state">Состояние, в которое будет переведён удалённый коммутационный аппарат.</param>
    ''' <exception cref="T:System.InvalidOperationException">Возникает в случае ошибки изменения состояния удалённого коммутационного аппарата.</exception>
    Public Sub Write(ByVal state As Switches.State)
        Dim command As Parts
        Select Case state
            Case Switches.State.O
                command = Parts.OffCommand
            Case Switches.State.I
                command = Parts.OnCommand
        End Select
        Try
            Using client As New WebClient
                client.BaseAddress = Me.ConnectionParts(Parts.ServiceURL).ToString
                Call client.DownloadData(Me.ConnectionParts(command))
            End Using
        Catch ex As Exception
            Throw New InvalidOperationException("Ошибка передачи данных удалённому устройству.", ex)
        End Try
    End Sub
    ''' <summary>
    ''' Подключается к удалённому коммутационному аппарату и устанавливает его в состояние, аналогичное состоянию аппарата-мастера.
    ''' </summary>
    ''' <param name="device">Коммутационный аппарат-мастер.</param>
    ''' <exception cref="T:System.InvalidOperationException">Возникает в случае ошибки изменения состояния удалённого коммутационного аппарата.</exception>
    Public Sub Write(ByVal device As ISwitchable)
        Call Write(device.State)
    End Sub

    ''' <summary>
    ''' Подключается к удалённому коммутационному аппарату и получает его текущее состояние.
    ''' </summary>
    ''' <returns><see cref="T:PWDA.Switches.State"/></returns>
    ''' <exception cref="T:System.InvalidOperationException">Возникает в случае ошибки чтения состояния удалённого коммутационного аппарата.</exception>
    Public Function Read() As State
        Dim response As String
        Try
            Using client As New WebClient
                client.BaseAddress = Me.ConnectionParts(Parts.ServiceURL).ToString
                response = client.DownloadString(Me.ConnectionParts(Parts.GetCommand))
            End Using
        Catch ex As Exception
            Throw New InvalidOperationException("Ошибка получения данных от удалённого устройства.", ex)
        End Try
        Dim ansString As String = Regex.Match(response, "(?<=\n\s?\n).*$").Value
        Return ansString.ToState

    End Function
End Class
