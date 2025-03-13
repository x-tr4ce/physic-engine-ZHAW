import pandas as pd
import matplotlib.pyplot as plt

# CSV-Datei einlesen (sollte im selben Verzeichnis liegen wie dieses Skript)
df = pd.read_csv('time_series.csv')

# ---------------------------
# Plot 1: Position vs. Zeit
# ---------------------------
plt.figure(figsize=(12, 6))

# Subplot für die y-Position (Höhe)
plt.subplot(2, 1, 1)
plt.plot(df['t'], df['y_Dart'], label='Dart (y)')
plt.plot(df['t'], df['y_Board'], label='Scheibe (y)')
plt.xlabel('Zeit (s)')
plt.ylabel('Position (m)')
plt.title('Vertikale Position (y) über die Zeit')
plt.legend()
plt.grid(True)

# Subplot für die z-Position (Tiefe/Richtung zum Ziel)
plt.subplot(2, 1, 2)
plt.plot(df['t'], df['z_Dart'], label='Dart (z)')
plt.plot(df['t'], df['z_Board'], label='Scheibe (z)')
plt.xlabel('Zeit (s)')
plt.ylabel('Position (m)')
plt.title('Tiefe Position (z) über die Zeit')
plt.legend()
plt.grid(True)

plt.tight_layout()
plt.show()

# ---------------------------
# Plot 2: Geschwindigkeit vs. Zeit
# ---------------------------
plt.figure(figsize=(12, 6))

# Subplot für die y-Geschwindigkeit
plt.subplot(2, 1, 1)
plt.plot(df['t'], df['DartVelY'], label='Dart Geschwindigkeit (y)')
plt.plot(df['t'], df['BoardVelY'], label='Scheibe Geschwindigkeit (y)')
plt.xlabel('Zeit (s)')
plt.ylabel('Geschwindigkeit (m/s)')
plt.title('Vertikale Geschwindigkeit (y) über die Zeit')
plt.legend()
plt.grid(True)

# Subplot für die z-Geschwindigkeit
plt.subplot(2, 1, 2)
plt.plot(df['t'], df['DartVelZ'], label='Dart Geschwindigkeit (z)')
plt.plot(df['t'], df['BoardVelZ'], label='Scheibe Geschwindigkeit (z)')
plt.xlabel('Zeit (s)')
plt.ylabel('Geschwindigkeit (m/s)')
plt.title('Tiefe Geschwindigkeit (z) über die Zeit')
plt.legend()
plt.grid(True)

plt.tight_layout()
plt.show()

# ---------------------------
# Plot 3: Beschleunigung vs. Zeit
# ---------------------------
plt.figure(figsize=(12, 6))

# Subplot für die y-Beschleunigung
plt.subplot(2, 1, 1)
plt.plot(df['t'], df['DartAccY'], label='Dart Beschleunigung (y)')
plt.plot(df['t'], df['BoardAccY'], label='Scheibe Beschleunigung (y)')
plt.xlabel('Zeit (s)')
plt.ylabel('Beschleunigung (m/s²)')
plt.title('Vertikale Beschleunigung (y) über die Zeit')
plt.legend()
plt.grid(True)

# Subplot für die z-Beschleunigung
plt.subplot(2, 1, 2)
plt.plot(df['t'], df['DartAccZ'], label='Dart Beschleunigung (z)')
plt.plot(df['t'], df['BoardAccZ'], label='Scheibe Beschleunigung (z)')
plt.xlabel('Zeit (s)')
plt.ylabel('Beschleunigung (m/s²)')
plt.title('Tiefe Beschleunigung (z) über die Zeit')
plt.legend()
plt.grid(True)

plt.tight_layout()
plt.show()
