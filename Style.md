# Стиль кода.
Ниже представлены требования к коду, чтобы он у нас был единым в плане оформления.

# Настройки Visual Studio.
В основном используется Visual Studio 2017. Если используется версия ниже и что-то не компилиться, есть три пути:
  1.  Установить 2017 студию
  2.  Установить расширение на 2015 студию, чтобы поддерживать C# 7.0
  3.  Вытаскивать за шкирку человека, который что-то написал, и вы вместе разбирались, почему не компилится проект.
  
# Требования к оформлению кода.
  В большинстве своём соблюдаем C# [нотацию](https://docs.microsoft.com/ru-ru/dotnet/csharp/programming-guide/inside-a-program/coding-conventions) с некоторыми уточнениями:
   
##   1.  Public, protected, private и локальные переменные.

Публичные переменные объявлябются с большой буквы. Защищённые и приватные - с нижнего подчёркивания.

Локальные переменные всегда именуются таким образом, чтобы полностью и целиком отразить свою цель. Нежелательно писать какое нибудь название temp, par, value. Лучше дайте немного более длинное название (не более слов 2-3), чтобы избежать раздумывания над тем, чтож под каким-то something имелось ввиду.

Нигде никогда никаких сокращений. Только в случаях, когда это общепринятые сокращения, например, ID, или IEEE. В остальных случаях пишем полностью. Получается слишком длинно - либо компонуем покороче, либо уж пишем огромную кракозябру.
        
      ```
      public object PublicMember;
      
      protected bool _isThisTrueCodeStyle;
      
      private int _first;
      ```
      
##   2. Код в классе должен быть разделён на зоны, то есть:
   
   ```
   //
   // Public members.
   //
   
   public object PublicMember;
   ...etc...
   
   //
   // Protected members.
   //
   
   protected object _protectedMember;
   ...etc...
   
   //
   // Private members.
   //
   
   private object _privateMember;
   ...etc...
   
   //
   // Constructors.
   //
   
   public Constructor();
   ...etc...
   
   //
   // Base implementation
   //
   
   //
   // Public methods.
   //
   
   //
   // Protected methods.
   //
   
   //
   // Private methods.
   //
   ```
   
   Желательно соблюдать подобную структуру в классе. Более подробно о том, какой порядок стоит соблюдать при написании кода, [описан тут](https://stackoverflow.com/a/310967). Это удобно не только для вас, но и для тех людей, которые будут читать, а может даже и тестировать ваш код.
   
   ##   3. Никакого дублирования кода. Нигде. Никогда.
   
   За дубляж кода - расстрел вне очереди. 4 курс, не дети подобные ошибки делать.
   
   ##   4. Название методов - по нотации с большой буквы. Сначала идёт глагол, потом всё остальное. Например:
   
   ```
   // Неправильно:
   void Do()
   {
      //...
   }
   
   void Cat()
   {
     //...
   }
   
   // Правильно:
   void DoSomeWork()
   {
      //...
   }
   ```
   
   Давайте полные, понятные названия методов, которые могут отразить суть выполняемого ими действия. Никаких temp, something или подобного в наименовании, только конкретно цель. Нет ничего страшного в том, что метод будет длиной в 5-6 слов, но в идеале должно быть не больше 3-4.
   
   ##   5. Комментарии кода.
   
   Классы, публичные методы и переменные комментируются с помощью summary (для быстрого снипета: /// + enter). Приватные, защищённые комментируем просто через //.
   
   ##   6. Разбиваем большие методы на более мелкие -> делаем код более самодокументированным.
   
   Как завещал дядя Макконел, которого я не читал, метод не должен быть в идеале больше, чем длина вашего монитора. Потому большие операции выносим в отдельные методы, по поводу наименования см. пункт 4.
   
   ##   7. Использование ключевых слов ref и out.
   
   Желательно в большинстве своём избегать подобных вещей, потому что метод всегда должен иметь фиксированный вход и выход. Используем только в крайних случаях, например, если добавленный вами сторонний фреймворк не может без этого обходиться.
   
   ##   8. Стараемся соблюдать принцип "разделяй и властвуй"
   
   Это довольно субъективно, но все же есть рекомендация делать так, чтобы класс выполнял только одну логику, так же и с методами: один метод исполняет только одну логику.
   
   Пример допустимого кода:
   
   ```
   // Неправильно
   void DoMyMethod()
   {
      // Here makes some logic...
      ...
      // Here makes some another logic...
      ...
   }
   
   // Правильно
   void DoMyMethod()
   {
        DoFirstLogicMethod();
        
        // Here makes it's own logic...
        
        DoSecondLogicMethod();
   }
   
   void DoFirstLogicMethod()
   {
       // Here makes it's own logic...
   }
   
   void DoSecondLogicMethod()
   {
      // Here makes it's own logic...
   }
   ```
   
   ##   9. Отступы
   
   При большом количестве параметров в методе или конструкторе, при его описании, делается перенос каждого параметра на новую строку с отступом в 8 пробелов от начала описания метода (от первой буквы модификатора). 
   
   ```
   public void DoSomeWork(
           object name,
           object age,
           object address,
           object ...)
   ```
  При вызове метода или создании нового класса с большим названием - делаем перенос на новую строчку после знака равно с отступом в 4 пробела от начала объявления. Если в этом методе/классе много параметров - в независимости от длины имени перенос на новую строку после равно как в предыдущем предложении + каждый параметр переносится на новую строку, с отступом в 4 пробела от начала метода.
   ```
    public void DoSomeWork()
    {
      // Some logic
      
      var myClass = 
          new ClassWithVeryLongName();
          
      var secondClass = 
          new MyClass(
              firstParam,
              secondParam,
              thirdParam,
              ...,
              lastParam);
              
      int resultOf = 
          BeginTransactionWith(
              first,
              second,
              ...
              last);
    }
   
   ```
   
 ##   10. По поводу tab и space...
 
 Рекомендуется использовать для отступа 4 space, а не 1 tab, поскольку в разных средах tab по разному воспринимается, где-то он состоит из 3 пробелов, где-то из 6 и так далее. Потому в настройках вижака:
 
 ```
  Для русского: Параметры -> Текстовый редактор -> С# -> Табуляция.  
  Отступы: Структура
  Табуляция:
    Размер интервала табулдяции: 4
    Размер отступа: 4
    RadioButton on "Вставлять пробелы"
    
  Для английского: Tools -> Options -> Text Editor -> C# -> Tabs.
  Indenting: Smart
  Tab:
    Tab size: 4
    Indent size: 4
    RadioButton on "Insert Spaces"
 ```
  ##   11.   Вложенность.
  Не стоит делать глубоко вложенные файлы. Одна папка может содержать 5-6 классов (либо: золотое правило 5 +/- 2)
  
# To be continued...

Список может обновляться и дополняться...
    