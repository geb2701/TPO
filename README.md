Sistema de notificaciones

Factory Crear modelos
Observer: escuchar eventos de partido
State: estados del patido
Adptarter: envio de mails
Stategy eleccion de partidos


Falta:
- Consultar partidos (2 endpoint - todos (hecho) e individual (recibe un id y devulve un dto igual que el de usuario))
- Confirmar (3 endpoint - confirmar al usuario logeado, a otro usuario, todos los de un partido) - la idea de que hagan jugador.confirmar hay que traer la informacion de partido y demas
- Sistema de notificaciones: en usuario debemos de alguna manera hacer un adaptador para ejecutar diferentes notificaciones apartir de  TipoNotificacion
- Cancelar partido (solo puede el que organizo el partido): pasamos un id y hacemos partido.cancelar hay que incluir los jugadores y usuario para que les informe
- Endpoint para decir partidos posible que puede inscribirse el usuario: el usuario debe poder consultar que partidos puede anotarse
- Inscribir a un partido: pasa un id de un patido e intentamnos incribrlo 