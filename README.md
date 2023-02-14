### Преглед
MathAdvisor е ASP.NET уебсайт, което позволява на потребителите да въвеждат уравнения и представя тяхното. Сайтът включва два проекта:

* Проект за уеб страництаа, която предоставя потребителски интерфейс за въвеждане на уравнения и показване на резултатите.
* API проект, който използва Math.Net Symbolics за решаване на уравнението и връща решението на уеб страницата.

### Проект за уеб страница
Проектът на уеб страницата е ASP.NET MVC проект, който предоставя потребителски интерфейс за въвеждане на уравнения. Проектът използва jQuery, за да прави заявки към Math_Advisor.API и да показва резултатите, без да презарежда страницата.

### API на проекта
Math_Advisor.API е проект на ASP.NET, който използва Math.NET Symbolics за решаване на уравнението и връща резултата на уеб страницата.

Изисквания на проекта:
1. Net.Core
2. ASP.NET

Можете да и просто компилирайте и стартирайте проекти Math_Advisor и Math_Advisor.API. Уеб страницата може да бъде достъпена на `https://localhost:443/` a апито на `https://localhost:8080/GetSolution`
