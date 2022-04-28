## TODO-LIST

xurban70:

Ondra_stuD: Front-end 1/2

kredenc: technik a upratovačka - BE + FE

TODO: dodelat controllery podle navrhu

FE 2/2

**/api/Auth/login:**
- zadne userName, ale email (prejmenovat)

- vracet roli přihlášeného uživatele

**/api/Auth/register:**
- zadne userName, ale email (prejmenovat)
- nebude se vyuzivat primo, ale v ramci Employee/Save
- teoreticky nemusi vracet nove vytvoreneho uzivatele, ale jen ok (minimalne nemusi vubec vracet passwordSalt) - ve finale, kdyz se to predela do toho Employee/Save tak uz je mi to jedno a kontrola operace bude na vas

**/api/Employee/Save**
- zavolat metodu register se zadanym emailem a heslem

**uplne chybi logout (otazka je jestli je treba)**

**kompletni zpracovani metod pro zachazeni s equipmenutem pokoje (napr. WIFI, TV, ...) - uz mozna asi ne, bude nejspis natvrdo**

odmazat WeatherForecast files

sepsat dokumentaci - doc.html

kazdy nove pouzity nuget package napsat do README


## STEZEJNI BODY:
 autorizace

 Sprava zamestnancu

## ZAPOMENUTO:
Sem dopiste co si vsimnete ze jeste chybi

## NEDĚLAT:
CRUD - informace o zakaznikovi (u recepcniho) - je to k nicemu


## Project for Advanced information Systems.

Implemented in .NET 6.0, EntityFrameworkCore 6.0.3

Application should contain REST API with EF Core Database Access, with vue.js frontend.

Should implement IS for managing Hotel.

## Parts of system

Hotel_PIS.DAL	- data access layer using EF Core

Hotel_PIS		- WEB API

Hotel_PIS.vue	- Frontend of application

https://docs.microsoft.com/en-us/visualstudio/javascript/tutorial-asp-net-core-with-vue?view=vs-2022

Authors: Petr Urbánek (HyperReap/xurban70)
