import pandas as pd
import numpy as np
import matplotlib.pyplot as plt

# 1) CSV einlesen
df = pd.read_csv('time_series-fixed-bumper.csv')

# 2) Zeit und Bewegungsgrößen extrahieren
t  = df['t'].values            # Zeit [s]
z  = df['z'].values            # Position auf z-Achse [m]
vz = df['vz'].values           # Geschwindigkeit auf z-Achse [m/s]

# 3) Beschleunigung mit numpy.gradient
az = np.gradient(vz, t)        # Beschleunigung [m/s²]

# 4) Subplots erstellen
fig, axs = plt.subplots(3, 1, figsize=(8, 12))

# Position
axs[0].plot(t, z)
axs[0].set_title('Position des Wagens über der Zeit')
axs[0].set_xlabel('t [s]')
axs[0].set_ylabel('z [m]')
axs[0].grid(True)

# Geschwindigkeit
axs[1].plot(t, vz)
axs[1].set_title('Geschwindigkeit des Wagens über der Zeit')
axs[1].set_xlabel('t [s]')
axs[1].set_ylabel('v_z [m/s]')
axs[1].grid(True)

# Beschleunigung
axs[2].plot(t, az)
axs[2].set_title('Beschleunigung des Wagens über der Zeit')
axs[2].set_xlabel('t [s]')
axs[2].set_ylabel('a_z [m/s²]')
axs[2].grid(True)

# Layout anpassen und anzeigen
plt.tight_layout()
plt.show()