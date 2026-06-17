# 📚 SIGEBI - Sistema de Gestión de Bibliotecas

El Sistema de Gestión de Bibliotecas (SIGEBI) es una solución de software estructurada, diseñada para resolver y automatizar los procesos institucionales derivados de la gestión manual de servicios bibliográficos. La misión del sistema es garantizar un acceso oportuno y controlado a los recursos, aplicando de forma uniforme las reglas de negocio en todos los canales de atención. 

## ✨ Características 

PrincipalesGestión del Catálogo y Usuarios: Registro, actualización y control de recursos bibliográficos y perfiles de usuarios.
Control de Acceso y Disponibilidad: Búsqueda en tiempo real con validación de condiciones de elegibilidad antes de otorgar un préstamo.
Préstamos y Devoluciones: Solicitud, aprobación, seguimiento y verificación de plazos.

Penalizaciones y Notificaciones: Aplicación automática de sanciones por retrasos y envío de notificaciones mediante servidor SMTP. 
Reportes y Trazabilidad: Mantenimiento de un registro de auditoría inalterable mediante interceptores nativos para asegurar la trazabilidad de las operaciones.

## 🏛️ Arquitectura del Sistema

El proyecto está construido bajo los principios de la Arquitectura Limpia (Clean Architecture). Este enfoque divide el sistema en responsabilidades estrictas mediante capas concéntricas, garantizando que el núcleo del negocio (Dominio y Aplicación) permanezca aislado e independiente de las tecnologías de interfaz de usuario, bases de datos y frameworks externos.

El flujo de dependencias es estrictamente unidireccional hacia el núcleo , utilizando Inyección de Dependencias y el patrón Repositorio para abstraer el acceso a los datos.

## 💻 Stack Tecnológico

El desarrollo de SIGEBI se fundamenta en un ecosistema robusto orientado a la escalabilidad:
Backend & Lógica Core

### Lenguaje:C# 
- Framework: .NET / ASP.NET Core  
- API: ASP.NET Core Web API (Punto de integración RESTful)  
- Seguridad: JSON Web Tokens (JWT) para autenticación y autorización  

### Persistencia de Datos
- ORM: Entity Framework Core (con patrón Unit of Work)  
- Base de Datos: Microsoft SQL Server  

### Presentación (Clientes Ligeros)

- Aplicación Web: ASP.NET MVC / Blazor (Interfaz de autoservicio) 
- Aplicación de Escritorio: WPF / WinForms (Interfaz operativa transaccional)   

## 👥 Autores y Contexto Académico

Desarrollado como proyecto final de Programación II. 

Institución: Instituto Tecnológico de Las Américas (ITLA)   
Integrantes:

- Enmanuel Antonio Tejada Díaz 
- Osvaldo Enrrique Reynoso Corona   
