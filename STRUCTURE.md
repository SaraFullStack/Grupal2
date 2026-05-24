# 📁 Estructura del Proyecto (Unity)

## 🎨 Art
Carpeta donde se guardan los recursos visuales del juego.

- **Animations** → animaciones del jugador, enemigos, portales y objetos.
  - **Player** → animaciones y controlador del jugador.
  - **Props** → animaciones de objetos como puentes, portales y coleccionables.
- **Characters** → modelos 3D de personajes y enemigos.
- **Environment** → modelos y piezas de escenario.
  - **Forest** → elementos del nivel del bosque.
  - **Lobby** → elementos del lobby principal.
- **Materials** → materiales usados en personajes, escenarios, objetos y efectos.
  - **Particles** → materiales para partículas.
  - **Portal** → material del portal.
  - **Skyboxes** → materiales de cielo.
- **Props** → modelos de objetos interactivos y decorativos.
- **Textures** → texturas generales del proyecto.
  - **Portal** → imágenes usadas para la animación del portal.
- **UI** → recursos visuales de la interfaz.
  - **Dialogs** → interfaz de diálogos.
  - **Fonts** → fuentes del juego.
  - **GameOver** → interfaz de Game Over.
  - **HUD** → interfaz principal durante la partida.
  - **Images** → iconos, botones y elementos gráficos.
  - **Menu** → interfaz del menú.
  - **Sounds** → sonidos usados en la UI.
  - **Start** → pantalla inicial.
  - **Tutorial** → interfaz de tutoriales.
  - **Videos** → vídeos usados en tutoriales.
- **VFX** → efectos visuales como partículas o polvo.

---

## 🔊 Audio
Recursos de sonido del proyecto.

- **Ambient** → sonidos de ambiente.
- **Music** → música del lobby y del nivel.
- **SFX** → efectos de sonido del jugador, ataques, cajas, daño, agua, tutoriales y Game Over.

---

## 📄 Docs
Documentación interna del proyecto.

- **memoria_vertical_slice.html** → memoria de la vertical slice.

---

## 🔌 Plugins
Plugins externos usados en el proyecto.

- **Demigiant / DOTween** → plugin usado para animaciones y movimientos por código.

---

## 🧱 Prefabs
Objetos ya preparados para reutilizar dentro de las escenas.

- **Camera** → prefabs relacionados con cámara y cursor.
- **Enemies** → prefabs de enemigos y setas enemigas.
- **Enviroment** → prefabs del entorno.
  - **Forest** → elementos reutilizables del bosque.
  - **Lobby** → elementos reutilizables del lobby.
- **Player** → prefab principal del jugador.
- **Props** → objetos interactivos y coleccionables.
  - **base** → prefab base del Energy Core.
- **UI** → managers de interfaz.
  - **Dialogs** → manager de diálogos.
  - **HUD** → manager del HUD.
  - **Tutorial** → manager y prefabs de tutoriales.
- **VFX** → efectos visuales reutilizables.

---

## 📦 Resources
Recursos cargados por Unity durante la ejecución.

- **DOTweenSettings.asset** → configuración de DOTween.
- **Sprites** → sprites auxiliares de personajes o animales.

---

## 🗺️ Scenes
Escenas principales del proyecto.

- **MainMenu** → menú principal.
- **Lobby** → zona central del juego.
- **Portal_Forest** → nivel del bosque.

---

## 🧩 ScriptableObjects
Datos configurables usados por los scripts.

- **Enemies** → datos de enemigos y mundo de agua.
- **Game** → datos generales del juego.
- **Interactables** → datos de cajas rompibles, objetos empujables y bola rodante.
- **Player** → datos de movimiento del jugador.

---

## 💻 Scripts
Código del proyecto organizado por sistemas.

- **Camera** → cámara, zoom, asignación automática y bloqueo de cursor.
- **Combat** → daño por contacto y lógica básica de combate.
- **Core** → scripts auxiliares, como animación de texturas.
- **Enemies** → comportamiento de enemigos, estados, detección, vida y setas venenosas.
- **Health** → sistema de vida y daño.
- **Input** → gestión de controles del jugador.
- **Interactables** → cajas empujables, objetos rompibles, puentes, coleccionables, ataques y bola rodante.
- **Player** → movimiento, empuje, respawn y recolección.
- **QoL** → scripts de apoyo para ajustar tamaño y dimensiones de objetos.
- **Scene** → portales, carga de escenas y lógica de cambio de zona.
  - **Traps** → trampas y daño ambiental.
- **UI** → lógica de interfaz.
  - **Dialogs** → sistema de diálogos.
  - **GameOver** → pantalla de Game Over.
  - **HUD** → HUD y contador de coleccionables.
  - **Menu** → menú de pausa/opciones.
  - **Start** → pantalla inicial.
  - **Tutorial** → tutoriales y ayudas visuales.
- **Visual** → ajustes visuales, colliders y scroll de UVs.

---

## ⚙️ Settings
Configuración técnica del proyecto.

- Configuración de URP.
- Perfiles de render para PC y móvil.
- Configuración de iluminación.
- **UI** → configuración de diálogos, HUD, Game Over, Start, Tutorial y localización.

---

## 🧪 TutorialInfo
Archivos generados por Unity/URP para información y ejemplos internos.

- **Icons** → iconos de información.
- **Scripts** → scripts de readme/editor generados por Unity.

---

## 🧰 UI Toolkit
Recursos base de UI Toolkit.

- **UnityThemes** → tema runtime por defecto de Unity.

---

## 🛠️ Otros archivos importantes

- **InputSystem_Actions.inputactions** → configuración del sistema de input.
- **New Terrain.asset** → asset de terreno usado en el proyecto.
- **AddressableAssetsData** → configuración de Addressables y localización.
