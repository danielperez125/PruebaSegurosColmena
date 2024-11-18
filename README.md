# ColmenaSegurosPrueba
Prueba técnica para Colmena Seguros.

Backend usado: .NetCore 6
MySql 8

Para inicializar el proyecto debemos descargar el codigo. 
git clone https://github.com/danielperez125/PruebaSegurosColmena.git

Al descargar el repositorio, se observará que el proyecto consta de una carpeta llamada App, que es en donde están las diferentes soluciones. Y una carpeta Sources, que es en donde está la imagen e ícono para los NuGets desarrollados.

Dentro de App, se encuentran 3 carpetas enumeradas a partir del número 2, (el número 1 correspondía al Front que no pude comenzar por tiempo).
2.API => Contiene el API, que a su vez está estructurada en una arqitectura por capas:
  1.Web => Contiene el proyecto con los Endpoint de logueo, Lista de Productos, CRUD de Cotizaciones y CRUD de Usuarios.
  2.Business => Capa que realiza todas las operaciones basadas en las reglas de negocio del proyecto.
  3.Data => Capa que permite la interacción del API con la BD. Para estas interacciones se basa en un ORM propio.
  4.Transversal => Capa que contiene clase global de uso en toda la aplicación. 

Cada Endpoint del API cuenta con seguridad JWT (a excepción del LoginController). Para interactuar con aglgún Endpoint, es necesario loguearse desde el /Login, enviando UserName y Password como parámetros de entrada, a fin de obtener un token de autenticación.
Dicho token se debe enviar en cada Endpoint como Bearer Token.
El tiempo de expiración del token se establece dentro del proyecto, en la clase de configuración 4.Transversal/ApiSettings.cs, en la propiedad estática config.ExpiresToken. Por default está en un minuto pero
se cuentan con varias opciones de tiempo a elegir.

El entorno de acceso para un usuario se configura desde la tabla de Usuarios, en la columna env. Allí se establecen los posibles ambientes: Dev, Test, Prod. Y las cadenas de conexión a cada ambiente se establecen
de manera cifrada en el JSON de configuración del API (appsettings.js)

Los diferentes controladores permiten establecer CRUD para funcionalidades de Users y Products (Productos ofrecidos por la aseguradora). La idea general era disponibilizar los Endpoint al Front, y que este pintara la información según la funcionalidad que le estuviese mostrando al usuario.

De manera Transversal se crearon NuGets para la autenticación basada en JWT. NuGets con utilidades (Cifrado y decifrado en AES256). NuGet de conexión a la BD MySql
Todo lo que está involucrado en el proyecto fue desarrollado desde cero.
