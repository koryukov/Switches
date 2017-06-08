Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports PWDA.Switches

' ===== Тестирование типа состояния коммутационного аппарата =====
<TestClass()> Public Class StateTests

    ' ----- Проверка создания правильных строк подключения -----
    <TestMethod()> Public Sub TrueConnectionString()

        Dim conn1 As String = "ServiceURL=http://stackoverflow.com/questions/26744093/is-there-a-way-to-inline-external-functions-into-an-ef-linq-query; OnCommand=/oncommand/on; OffCommand=?ligth=false;GetCommand=/?ligth"
        Dim conn2 As String = "ServiceURL=http://stackoverflow.com/questions/26744093/is-there-a-way-to-inline-external-functions-into-an-ef-linq-query;OnCommand=/oncommand/on;OffCommand=?ligth=false;GetCommand=/?ligth;"
        Dim conn3 As String = "ServiceURL=http://stackoverflow.com; OnCommand=/oncommand/on; OffCommand=?ligth=false;GetCommand=/?ligth"
        Dim conn4 As String = "ServiceURL=http://localhost:567; OnCommand=/oncommand/on; OffCommand=?ligth=false;GetCommand=/?ligth"
        Dim conn5 As String = "ServiceURL=http://user@passwordlocalhost:567; OnCommand=/oncommand/on; OffCommand=?ligth=false; GetCommand=/?ligth"
        Try
            Dim conn As New Connection(conn1)
            conn.ConnectionString = conn2
            conn.ConnectionString = conn3
            conn.ConnectionString = conn4
            conn.ConnectionString = conn5
        Catch ex As Exception
            Assert.Fail("Возникает ошибка при задании верной строки подключения")
        End Try
    End Sub

    ' ----- Проверка создания неправильных строк подключения
    <TestMethod()> Public Sub FalseConnectionString()
        Dim key As Boolean = False
        Dim cs As String = "ServiceURL=http://stackoverflow.com/questions/26744093/is-there-a-way-to-inline-external-functions-into-an-ef-linq-query; OffCommand=?ligth=false;GetCommand=/?ligth"

        Try
            Dim conn As New Connection(cs)
        Catch ex As Exception
            key = True
        End Try
        Assert.IsTrue(key, "Создан объект имеющий не все сегменты строки подключения.")

        cs = "ServiceURL=http://stackoverflow.com/questions/26744093/is-there-a-way-to-inline-external-functions-into-an-ef-linq-query; OnCommand=http://myhost.com/oncommand/on; OffCommand=?ligth=false;GetCommand=/?ligth"
        Try
            Dim conn As New Connection(cs)
        Catch ex As Exception
            key = True
        End Try
        Assert.IsTrue(key, "Создан объект, команды которого обращаются к разным хостам.")

        cs = "ServiceURL=/stackoverflow.com/questions/26744093/is-there-a-way-to-inline-external-functions-into-an-ef-linq-query; OnCommand=/oncommand/on; OffCommand=?ligth=false;GetCommand=/?ligth"
        Try
            Dim conn As New Connection(cs)
        Catch ex As Exception
            key = True
        End Try
        Assert.IsTrue(key, "Создан объект, обращающийся по относитальному URL")
    End Sub

    ' ----- Тестирование ссылочных типов состояний коммутационного аппарата
    <TestMethod()> Public Sub StateReference()
        Dim a As New Contact("NC", State.I)
        Dim b As New Contact("NO", State.O)
        Assert.AreNotSame(a.State, b.State, "Неверное сравнение неравных объектов.")
        Assert.AreNotSame(a.State, b.State, "Разные переменные ссылаются на один объект.")
        a.State = b.State
        Assert.AreEqual(a.State, b.State, "Неверное сравнение равных объектов.")
        Assert.AreSame(a.State, b.State, "Разные переменные должны ссылаться на один объект.")
        b.State = State.I
        Assert.AreNotEqual(a.State, b.State, "Неверное сравнение неравных объектов.")
        Assert.AreNotSame(a.State, b.State, "Разные переменные ссылаются на один объект.")
    End Sub

    ' ----- Проверка операторов сравнения для состояний коммутационных аппаратов -----
    <TestMethod()> Public Sub StateCompare()
        Assert.IsTrue(State.I = State.I, "Ошибка при проверке оператора =. Неверное сравнение равных объектов.")
        Assert.IsTrue(State.O = State.O, "Ошибка при проверке оператора =. Неверное сравнение равных объектов.")
        Assert.IsFalse(State.I = State.O, "Ошибка при проверке оператора =. Неверное сравнение неравных объектов.")
        Assert.IsTrue(State.I <> State.O, "Ошибка при проверке оператора <>. Неверное сравнение неравных объектов.")
        Assert.IsFalse(State.I <> State.I, "Ошибка при проверке оператора <>. Неверное сравнение равных объектов.")
        Assert.IsTrue(State.I.Equals(State.I), "Ошибка при проверке метода Equals. Неверное сравнение равных объектов.")
        Assert.IsTrue(State.O.Equals(State.O), "Ошибка при проверке метода Equals. Неверное сравнение равных объектов.")
        Assert.IsFalse(State.I.Equals(True), "Ошибка при проверке метода Equals. Неверное сравнение неравных объектов.")
        Assert.IsTrue(State.I Is State.I, "Ошибка при проверке оператора Is. Неверное сравнение равных объектов.")
        Assert.IsTrue(State.O Is State.O, "Ошибка при проверке оператора Is. Неверное сравнение равных объектов.")
        Assert.IsFalse(State.O Is State.I, "Ошибка при проверке оператора Is. Неверное сравнение неравных объектов.")
        Assert.IsFalse(State.O Is New Object, "Ошибка при проверке оператора Is. Неверное сравнение неравных объектов.")
        Dim a As State = State.I
        Dim b As State = State.I
        Assert.AreEqual(a, b, "Неравентсво присвоенных значений.")
        Assert.AreSame(a, b, "Переменные ссылаются на разные объекты.")
        b = State.O
        Assert.AreNotEqual(a, b, "Равенство присвоенных значений.")
        Assert.AreNotSame(a, b, "Переменные ссылаются один объект.")
    End Sub

    ' ----- Проверка работы логических операторов для состояний коммутационных аппаратов -----
    <TestMethod()> Public Sub StateLogic()
        Assert.AreEqual(State.I And State.I, State.I, "Ошибка при проверке оператора AND. Два замкнутых контакта должны быть эквивалентны замкнутому.")
        Assert.AreEqual(State.I And State.O, State.O, "Ошибка при проверке оператора AND. Замкнутый и разомкнутый контакты должны быть эквивалентны разомкнутому.")
        Assert.AreEqual(State.O And State.O, State.O, "Ошибка при проверке оператора AND. Два разомкнутых контакта должны быть эквивалентны разомкнутому.")
        Assert.AreEqual(State.I Or State.I, State.I, "Ошибка при проверке оператора OR. Два замкнутых контакта должны быть эквивалентны замкнутому.")
        Assert.AreEqual(State.I Or State.O, State.I, "Ошибка при проверке оператора OR. Замкнутый и разомкнутый контакты должны быть эквивалентны замкнутому.")
        Assert.AreEqual(State.O Or State.O, State.O, "Ошибка при проверке оператора OR. Два разомкнутых контакта должны быть эквивалентны разомкнутому.")
        Assert.AreEqual(State.I Xor State.I, State.O, "Ошибка при проверке оператора XOR. Два замкнутых контакта должны быть эквивалентны разомкнутому.")
        Assert.AreEqual(State.O Xor State.O, State.O, "Ошибка при проверке оператора XOR. Два разомкнутых контакта должны быть эквивалентны разомкнутому.")
        Assert.AreEqual(State.I Xor State.O, State.I, "Ошибка при проверке оператора XOR. Замкнутый и разомкнутый контакты должны быть эквивалентны замкнутому.")
        Assert.AreEqual(State.O Xor State.I, State.I, "Ошибка при проверке оператора XOR. Замкнутый и разомкнутый контакты должны быть эквивалентны замкнутому.")
        Assert.AreEqual(Not (State.O), State.I, "Ошибка при проверке оператора NOT. Инверсия разомкнутого контакта должна быть эквивалентна замкнутому контакту.")
        Assert.AreEqual(Not (State.I), State.O, "Ошибка при проверке оператора NOT. Инверсия замкнутого контакта должна быть эквивалентна разомкнутому контакту.")
    End Sub

    ' ----- Проверка преобразования типов State -----
    <TestMethod> Public Sub StateConversion()
        Dim bOn As Boolean = True
        Dim bOff As Boolean = False

        Dim strOn As String = "On"
        Dim strOff As String = "OFF"
        Dim strTrue As String = "TRUE"
        Dim strFalse As String = "faLsE"
        Dim strI As String = "I"
        Dim strO As String = "o"
        Dim str1 As String = "1"
        Dim str0 As String = "0"
        Dim str As String = "NO"

        Assert.AreEqual(State.I, bOn.ToState, "Неверное преобразование значения True")
        Assert.AreEqual(State.I, strOn.ToState, "Неверное преобразование строки """ + strOn + """")
        Assert.AreEqual(State.I, strTrue.ToState, "Неверное преобразование строки """ + strTrue + """")
        Assert.AreEqual(State.I, strI.ToState, "Неверное преобразование строки """ + strI + """")
        Assert.AreEqual(State.I, str1.ToState, "Неверное преобразование строки """ + str1 + """")

        Assert.AreEqual(State.O, bOff.ToState, "Неверное преобразование значения False")
        Assert.AreEqual(State.O, strOff.ToState, "Неверное преобразование строки """ + strOff + """")
        Assert.AreEqual(State.O, strFalse.ToState, "Неверное преобразование строки """ + strFalse + """")
        Assert.AreEqual(State.O, strO.ToState, "Неверное преобразование строки """ + strO + """")
        Assert.AreEqual(State.O, str0.ToState, "Неверное преобразование строки """ + str0 + """")

        Assert.IsNull(str.ToState, "Неверное преобразование строки """ + str + """")

        Assert.AreEqual(True, State.I.ToBoolean, "Неверное преобразование включенного состояния в Boolean")
        Assert.AreEqual(False, State.O.ToBoolean, "Неверное преобразование выключенного состояния в Boolean")
    End Sub

    
End Class

' ===== Проверка реле =====
<TestClass()> Public Class RelayTests

    ' ----- Проверка ограничения доступа к контактам реле -----
    <TestMethod()> Public Sub ContactEdit()
        Dim contacts As New List(Of Contact)
        contacts.Add(New Contact("NC", State.I))
        contacts.Add(New Contact("NO", State.O))
        Dim relay As New Relay("My Relay", State.O, contacts)
        contacts.Clear()

        relay.Contacts.Add(New Contact("New NO Contact", State.O))
        contacts = relay.GetContacts(State.O)

    End Sub

    ' ----- Тестирование переключений реле -----
    <TestMethod()> Public Sub Relay()
        Dim contacts As New List(Of Contact)
        contacts.Add(New Contact("NC", State.I))
        contacts.Add(New Contact("NO", State.O))
        Dim relay As New Relay("My Relay", State.O, contacts)

        Assert.AreEqual(relay.State, State.O, "Ошибка инициализации. После инициализации реле должно находиться в выключенном состоянии")
        Assert.IsTrue(relay.IsOFF, "Ошибка метода IsOff. После инициализации значение должно быть ИСТИНА")
        Assert.IsFalse(relay.IsON, "Ошибка метода IsON. После инициализации значение должно быть ЛОЖЬ")

        Assert.AreEqual(relay.GetContact("NO").State, State.O, "Ошибка инициализации. Нормально открытый контакт должен быть разомкнут.")
        Assert.IsFalse(relay.GetContact("NO").IsON, "Ошибка метода IsON. Нормально открытый контакт после инициализации не может быть замкнут.")
        Assert.IsTrue(relay.GetContact("NO").IsOFF, "Ошибка метода IsON. Нормально открытый контакт после инициализации должен быть разомкнут.")

        Assert.AreEqual(relay.GetContact("NC").State, State.I, "Ошибка инициализации. Нормально закрытый контакт должен быть замкнут.")
        Assert.IsTrue(relay.GetContact("NC").IsON, "Ошибка метода IsON. Нормально закрытый контакт после инициализации должен быть замкнут.")
        Assert.IsFalse(relay.GetContact("NC").IsOFF, "Ошибка метода IsON. Нормально закрытый контакт после инициализации не может быть разомкнут.")

        relay.Switch()
        Assert.IsTrue(relay.IsON, "Ошибка переключения. Реле должно перейти во включенное состояние, метод IsON указывает обратное.")
        Assert.IsFalse(relay.IsOFF, "Ошибка переключения. Реле должно перейти во включенное состояние, метод IsOFF указывает обратное.")
        Assert.AreEqual(relay.State, State.I, "Ошибка переключения. Реле должно перейти во включенное состояние.")

        Assert.AreEqual(relay.GetContact("NO").State, State.I, "Ошибка переключения. Нормально открытый контакт должен замкнуться.")
        Assert.IsFalse(relay.GetContact("NO").IsOFF, "Ошибка переключения. Нормально открытый контакт должен замкнуться, метод IsOFF указывает обратное.")
        Assert.IsTrue(relay.GetContact("NO").IsON, "Ошибка переключения. Нормально открытый контакт должен замкнуться, метод IsON указывает обратное.")

        Assert.AreEqual(relay.GetContact("NC").State, State.O, "Ошибка переключения. Нормально закрытый контакт должен разомкнуться.")
        Assert.IsTrue(relay.GetContact("NC").IsOFF, "Ошибка переключения. Нормально закрытый контакт должен разомкнуться, метод IsOFF указывает обратное.")
        Assert.IsFalse(relay.GetContact("NC").IsON, "Ошибка переключения. Нормально закрытый контакт должен разомкнуться, метод IsON указывает обратное.")

        relay.SwitchON()
        Assert.IsTrue(relay.IsON, "Ошибка переключения SwitchON. Реле должно перейти во включенное состояние, метод IsON указывает обратное.")
        Assert.IsFalse(relay.IsOFF, "Ошибка переключения SwitchON. Реле должно перейти во включенное состояние, метод IsOFF указывает обратное.")
        Assert.AreEqual(relay.State, State.I, "Ошибка переключения SwitchON. Реле должно перейти во включенное состояние.")

        Assert.AreEqual(relay.GetContact("NO").State, State.I, "Ошибка переключения SwitchON. Нормально открытый контакт должен замкнуться.")
        Assert.IsFalse(relay.GetContact("NO").IsOFF, "Ошибка переключения SwitchON. Нормально открытый контакт должен замкнуться, метод IsOFF указывает обратное.")
        Assert.IsTrue(relay.GetContact("NO").IsON, "Ошибка переключения SwitchON. Нормально открытый контакт должен замкнуться, метод IsON указывает обратное.")

        Assert.AreEqual(relay.GetContact("NC").State, State.O, "Ошибка переключения SwitchON. Нормально закрытый контакт должен разомкнуться.")
        Assert.IsTrue(relay.GetContact("NC").IsOFF, "Ошибка переключения SwitchON. Нормально закрытый контакт должен разомкнуться, метод IsOFF указывает обратное.")
        Assert.IsFalse(relay.GetContact("NC").IsON, "Ошибка переключения SwitchON. Нормально закрытый контакт должен разомкнуться, метод IsON указывает обратное.")

        relay.SwitchOFF()
        Assert.IsFalse(relay.IsON, "Ошибка переключения SwitchOFF. Реле должно перейти в выключенное состояние, метод IsON указывает обратное.")
        Assert.IsTrue(relay.IsOFF, "Ошибка переключения SwitchOFF. Реле должно перейти в выключенное состояние, метод IsOFF указывает обратное.")
        Assert.AreEqual(relay.State, State.O, "Ошибка переключения SwitchOFF. Реле должно перейти в выключенное состояние.")

        Assert.AreEqual(relay.GetContact("NO").State, State.O, "Ошибка переключения SwitchOFF. Нормально открытый контакт должен разомкнуться.")
        Assert.IsTrue(relay.GetContact("NO").IsOFF, "Ошибка переключения SwitchOFF. Нормально открытый контакт должен разомкнуться, метод IsOFF указывает обратное.")
        Assert.IsFalse(relay.GetContact("NO").IsON, "Ошибка переключения SwitchOFF. Нормально открытый контакт должен разомкнуться, метод IsON указывает обратное.")

        Assert.AreEqual(relay.GetContact("NC").State, State.I, "Ошибка переключения SwitchOFF. Нормально закрытый контакт должен замкнуться.")
        Assert.IsFalse(relay.GetContact("NC").IsOFF, "Ошибка переключения SwitchOFF. Нормально закрытый контакт должен замкнуться, метод IsOFF указывает обратное.")
        Assert.IsTrue(relay.GetContact("NC").IsON, "Ошибка переключения SwitchOFF. Нормально закрытый контакт должен замкнуться, метод IsON указывает обратное.")
    End Sub

    ' ----- Проверка соедиенения с коммутационным аппаратом -----
    <TestMethod> Public Sub Connection()
        Dim onState As State
        Dim offState As State
        Try
            Dim c As New Connection("ServiceURL=http://192.98.39.150:8008/switcher; OnCommand=?light=on; OffCommand=?light=off; GetCommand=?light")
            c.Write(State.O)
            c.Write(State.I)
            onState = c.Read
            c.Write(State.O)
            offState = c.Read
        Catch ex As Exception
            Assert.Fail("При создании подключения возникла ошибка. " + ex.Message)
            Exit Sub
        End Try

        Assert.AreEqual(State.I, onState, "Не удалось переключить удалённое устройство в состояние ON")
        Assert.AreEqual(State.O, offState, "Не удалось переключить удалённое устройство в состояние OFF")
    End Sub

    ' ----- Проверка работы c удалённым коммутационным аппаратом -----
    <TestMethod> Public Sub NetworkRelay()
        Dim relay As New NetworkRelay( _
            "My Remote Relay", _
            "ServiceURL=http://192.98.39.150:8008/switcher; OnCommand=?light=on; OffCommand=?light=off; GetCommand=?light", _
            New Contact("Light", State.O))
        relay.SwitchOFF()
        relay.Upload()
        relay.Download()
        Dim state1 As State = relay.State   ' OFF
        relay.Switch()
        Dim state2 As State = relay.State   ' ON
        relay.Download()
        Dim state3 As State = relay.State   ' OFF
        relay.Switch()
        relay.Upload()
        relay.Download()
        Dim state4 As State = relay.State   ' ON
        relay.State = State.O
        relay.Upload()
        relay.Download()
        Dim state5 As State = relay.State   ' OFF
        relay.SwitchON()
        relay.Upload()
        relay.Download()
        Dim state6 As State = relay.State   ' ON
        relay.SwitchOFF()
        relay.Upload()
        relay.Download()
        Dim state7 As State = relay.State   ' OFF
        relay.SwitchTo(State.I)
        relay.Upload()
        relay.Download()
        Dim state8 As State = relay.State   ' ON
        relay.SwitchON()
        relay.Upload()
        relay.Download()
        Dim state9 As State = relay.State   ' ON
        relay.State = State.I
        relay.Upload()
        relay.Download()
        Dim state10 As State = relay.State  ' ON

        Assert.AreEqual(State.O, state1, "Не удалось перевести удалённое реле в выключенное состояние в начале теста.")
        Assert.AreEqual(State.O, state3, "Удалённое реле не должно переключаться без синхронизации.")
        Assert.AreEqual(State.I, state4, "Не удалось переключить удалённое реле командой Switch.")
        Assert.AreEqual(State.O, state5, "Не удалось передать удалённому реле состояние OFF.")
        Assert.AreEqual(State.I, state6, "Не удалось переключить удалённое реле командой SwitchON.")
        Assert.AreEqual(State.O, state7, "Не удалось переключить удалённое реле командой SwitchOFF.")
        Assert.AreEqual(State.I, state8, "Не удалось включить удалённое реле командой SwitchTo.")
        Assert.AreEqual(State.I, state9, "Неверное переключение реле командой SwitchON.")
        Assert.AreEqual(State.I, state10, "Неверное переключение реле при передаче включённого состояния.")
    End Sub

    ' ----- Проверка автоматической синхронизации c удалённым коммутационным аппаратом -----
    <TestMethod> Public Sub NetworkRelaySync()
        Dim relay As New NetworkRelay( _
            "My Remote Relay", _
            "ServiceURL=http://192.98.39.150:8008/switcher; OnCommand=?light=on; OffCommand=?light=off; GetCommand=?light", _
            New Contact("Light", State.O))
        relay.AutoSync = True

        relay.SwitchOFF()
        relay.Download()
        Dim state1 As State = relay.State   ' OFF

        relay.Switch()
        Dim state2 As State = relay.State   ' ON

        relay.Download()
        Dim state3 As State = relay.State   ' ON

        relay.State = State.O
        relay.Download()
        Dim state4 As State = relay.State   ' OFF

        relay.SwitchON()
        relay.Download()
        Dim state6 As State = relay.State   ' ON

        relay.SwitchOFF()
        relay.Download()
        Dim state7 As State = relay.State   ' OFF

        relay.SwitchTo(State.I)
        relay.Download()
        Dim state8 As State = relay.State   ' ON

        relay.SwitchON()
        relay.Download()
        Dim state9 As State = relay.State   ' ON

        relay.State = State.I
        relay.Download()
        Dim state10 As State = relay.State  ' ON

        Assert.AreEqual(State.O, state1, "Не удалось перевести удалённое реле в выключенное состояние в начале теста.")
        Assert.AreEqual(State.I, state3, "Не удалось переключить удалённое реле командой Switch.")
        Assert.AreEqual(state2, state3, "Состояние локального экземпляра и удалённого реле различны.")
        Assert.AreEqual(State.O, state4, "Не удалось упередать удалённому реле состояние OFF.")
        Assert.AreEqual(State.I, state6, "Не удалось переключить удалённое реле командой SwitchON.")
        Assert.AreEqual(State.O, state7, "Не удалось переключить удалённое реле командой SwitchOFF.")
        Assert.AreEqual(State.I, state8, "Не удалось включить удалённое реле командой SwitchTo.")
        Assert.AreEqual(State.I, state9, "Неверное переключение реле командой SwitchON.")
        Assert.AreEqual(State.I, state10, "Неверное переключение реле при передаче включённого состояния.")
    End Sub
End Class