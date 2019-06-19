# PhotoGallery-ASP.NET
Розроблена веб-програма для створення і наповнення персональних тематичних фотогалерей користувача (проект ASP.NET Web Application). Сайт дозволяє анонімному веб-клієнту передивлятись фотогалереї, а авторизованому веб-клієнту (власнику фотогалерей) – створювати, змінювати і вилучати фотогалереї і у такий спосіб поширювати своїм друзям і колегам файли растрових зображень популярних форматів .JPG, .BMP, .PNG, .GIF.

page1.aspx	- стартова сторінка для анонімного веб-користувача з переліком назв всіх фотогалерей. Перелік  представлений у вигляді таблиці (об’єкт <asp:Table ... asp>) або у будь-який інший спосіб, причому одна фотогалерея займає один цілий рядок, який завершується кнопкою «ВІДКРИТИ». Нумерація фотогалерей на цій анонімній сторінці НЕ відображається, але послідовність фотогалерей у цьому списку є чітко визначеною за допомогою поля Gallery_Number відповідної MSSQL-таблиці GALLERY_LIST:

ID	Gallery_Number	Gallery_Name
1	2	Матеріали конференції в Будапешті, березень 2017
2	1	Звітні документи за І квартал 2017
4	4	Відпустка в Італії, липень 2017
5	3	Порядок встановлення програми «Access-2016»

Окрім переліку назв всіх фотогалерей в нижній частині сторінки відображені два текстові поля для логіна і пароля, а також кнопка «ВХІД» для авторизації власника фотогалерей. Пароль під час друку відображається не справжніми символами пароля, а символами «точка». В разі невдало введеного пароля кнопка буде надруковане червоне жирне повідомлення «Комбінація логіна/пароля є недійсною».

Page2.aspx	- сторінка, на яку переходить анонімний користувач коли натискає кнопку«ВІДКРИТИ» напроти конкретної фотогалереї. На цій сторінці користувач бачить зверху назву фотогалереї, кнопку «НАЗАД» і мініатюрні версії (англ. thumbnails) усіх без виключення зображень обраної фотогалереї, геометричний розмір яких не перевищує розмір сірникового коробка, а піксельний розмір складає приблизно 200 х 150 точок (200 ширина і 150 висота):
 
При цьому кожне зображення зберігається на сервері у вигляді ДВОХ окремих файлів – основного файла-зображення з оригінальним піксельним розміром, наприклад 4000 х 3000 точок і його міні-версії розміром 200 х 150 точок, при цьому розмір файла міні-версії істотно менший за розмір оригінального файла. Файли кожної фотогалереї зберігаються в ОКРЕМОМУ підкаталозі. При наведенні миші на зменшену версію конкретного зображення одразу з’являється підказка поверх цього зображення з коротким описом чи поясненням цього зображення, див. рисунок.

Коли користувач не тільки наводить, а ще і клацає мишею на міні-версію зображення, він відкриває нове окреме вікно браузера, в якому відображається повна картинка оригінального піксельного розміру, тобто на увесь екран користувача. Також на цій сторінці/вікні наведено для користувача абсолютне URL-посилання на обрану повну картинку для того, щоб користувач міг скопіювати це посилання в буфер обміну і надіслати посилання «іншому користувачеві», який, в свою чергу, міг би відкрити отримане посилання в новому вікні браузера і переглянути повну версію тієї ж картинки без відвідування фотогалереї.   Перехід до наступного зображення можна реалізувати у будь-який спосіб. 
Всі файли зображень повинні міститись у MSSQL-таблиці PICTURE_LIST змісту:


ID	Gallery_ID	Picture_Number	Description	Mini-Version	Full-Version
1	4	4	Міський пейзаж	01M.jpg	01F.jpg
2	4	3	Аеропорт в Шанхаї	02M.jpg	02F.jpg
4	4	1	Пам’ятник Мао Дзе Дуну 	03M.jpg	03F.jpg
5	4	2	Нічний Шанхай	04M.jpg	04F.jpg

Послідовність міні-версій зображень також повинна бути визначена відповідно до параметру Picture_Number. Анонімний користувач не може змінювати це поле, а отже, не може переставляти місцями картинки в межах фотогалереї.

Якщо на сторінці Page1.aspx користувач вводить дійсну комбінацію логіна і пароля – він переходить на сторінку Page4.aspx, яка є майже точною копією сторінки Page1.aspx, але для авторизованого користувача, а отже в нижній частині сторінки замість текстових полів для логіна і пароля і кнопки «ВХІД» є кнопка «ВИХІД». Нумерація фотогалерей на сторінці Page4.aspx повинна відображатись. Логін і пароль адміна можна заданий в самому C#-коді. Крім того, сторінка містить текстове поле для введення назви фотогалереї і кнопку «СТВОРИТИ ФОТОГАЛЕРЕЮ», при натисканні на яку створюється нова порожня фотогалерея, тобто новий запис таблиці GALLERY_LIST і новий порожній підкаталог. Нарешті, кожен рядок списку фотогалерей містить не тільки кнопку/гіперпосилання «ВІДКРИТИ», а ще кнопки/гіперпосилання «UP», «DOWN» і «DELETE». При натисканні кнопки «DELETE» вилучається фотогалерея як запис таблиці GALLERY_LIST, вилучаються весь підкаталог з файлами фотогалереї і всі відповідні записи таблиці PICTURE_LIST. Кнопка «UP» зменшує на 1 поле  Gallery_Number відповідної фотогалереї, а номер попередньої за цим номером фотогалереї – збільшує на 1. В результаті фотогалерея підіймається на 1 сходинку вверх в списку фотогалерей. Аналогічно, кнопка «DOWN» збільшує на 1 поле Gallery_Number відповідної фотогалереї, а номер наступної за цим номером фотогалереї – зменшує на 1. В результаті фотогалерея спускається на 1 сходинку вниз в списку фотогалерей. 

Коли авторизований користувач натискає кнопку «ВІДКРИТИ», він переходить на сторінку page5.aspx, схожу за змістом на сторінку Page2.aspx. Зверху повинно бути текстове поле для введення підказки нової фотографії, об’єкт <asp:FileUpload ... asp> і кнопка «ДОДАТИ ЗОБРАЖЕННЯ». При натисканні на цю кнопку на сервер передається файл-картинка, зберігається на сервері у підкаталог відповідної фотогалереї, а також автоматично конвертується у міні-версію цієї картинки з піксельним розміром 200 х 150 точок і ця міні-версія зберігається як другий окремий файл-картинка з іншим ім’ям. При цьому створюється новий запис таблиці PICTURE_LIST. Конвертацію картинки виконайте за допомогою   .NET-бібліотеки System.Drawing або у інший спосіб. Окрім того, біля кожної міні-версії картинки повинні бути кнопки/гіперпосилання «UP», «DOWN» і «DELETE». При натисканні кнопки «DELETE» вилучається картинка як пара двох файлів (оригінального і зменшеного розміру), а також запис таблиці PICTURE_LIST. Кнопки/гіперпосилання «UP» і «DOWN» дозволяють переміщувати картинку в межах усієї фотогалереї шляхом збільшення або зменшення поля Picture_Number таблиці PICTURE_LIST аналогічно тому, як переміщувались фотогалереї в таблиці GALLERY_LIST завдяки збільшення/зменшення поля Gallery_Number.
