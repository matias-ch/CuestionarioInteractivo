# Parte 2: Aplicación con Interfaz de Usuario
## Descripción
En este repositorio se encuentra la segunda parte del examen técnico.
Esta es una aplicación web desarrollada en .NET Blazor Server que permite a los usuarios participar en cuestionarios interactivos.

## Características principales
* Realizado con Blazor Server.

* Interfaz responsive.

* Carga de cuestionarios desde un archivo JSON.
    * Esto permite flexibilidad para modificar las preguntas sin necesidad de cambiar el código.
    * Para disminuir el impacto en el hardware solo se lee el archivo cuestionario.json al iniciar la aplicación. 
        * El archivo queda en memoria durante la ejecución de la aplicación para ser rápidamente leido por los usuarios que realizan el cuestionario.

* Tamaño dinámico del cuestionario
    * Puede haber cuantas preguntas se requieran.
    * Cada pregunta puede tener cuantas opciones se requieran.
    * Se puede asignar a una o varias opciones como correctas.

* Envío de correos electrónicos.
    * Se incluye un servicio de email para enviar los resultados de preguntas del cuestionario.
    * Para que funcione este servicio se debe ingresar la configuración del servidor SMTP en el archivo appsettings.json.

* Las respuestas pueden ser mostradas utilizando al menos cuatro soportes diferentes. Estos son:
    * Abrir un Pop-up (alert).
    * Abrir una nueva pestaña.
    * Mostrar en la misma página.
    * Enviar un email.

* El usuario puede guardar sus respuestas en un archivo de texto y luego cargar ese archivo para revisar sus respuestas.
    * Nota: La funcionalidad de guardar y cargar respuestas podría no funcionar correctamente en dispositivos móviles. Sin embargo, en navegadores de escritorio opera sin inconvenientes. Este comportamiento también se presenta en otros proyectos que vi desarrollados con .NET 8, y tengo pendiente su corrección.

## Envío de correos electrónicos
Para poder enviar emails se deben ingresar los siguientes datos que se encuentran en appsettings.json en la raíz del proyecto:

```json
"SmtpSettings": {
    "Host": "hostsmtp.com",
    "Port": 587,
    "Username": "usuario@email.com",
    "Password": "contraseña",
    "EnableSsl": true
  }
```

La configuración ingresada actualmente solo sirve de ejemplo. No corresponde a un host real de SMTP. Se debe completar con datos reales, sino no va a funcionar el servicio de email.

## Carga de cuestionarios desde un archivo JSON
Las preguntas y sus opciones se cargan desde el archivo cuestionario.json en la raíz del proyecto. El formato es el siguiente:

```json
[
  {
    "Pregunta": "Blazor en .NET",
    "Opciones": [
      {
        "Texto": "Es un framework para construir interfaces web con C# y Razor",
        "EsCorrecta": true
      },
      {
        "Texto": "Es un motor de base de datos para aplicaciones .NET",
        "EsCorrecta": false
      },
      {
        "Texto": "Puede ejecutarse tanto en el servidor como en el cliente.",
        "EsCorrecta": true
      }
    ],
    "MostrarSolucionEn": 0
  }
]
```

Actualmente, como pide la consigna, hay cargadas cinco preguntas técnicas relacionadas con .NET, SQL Server y Azure DevOps.