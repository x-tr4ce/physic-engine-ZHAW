# plot_impulse.py

import pandas as pd
import numpy as np
import matplotlib.pyplot as plt

# 1) CSV einlesen
df = pd.read_csv('time_series-friction.csv')

# 2) Daten extrahieren
t       = df['t'].values
vz      = df['vz'].values
F_left  = df['F_left'].values
vz_bumper = df['vz_bumper'].values
# F_right = df['F_right'].values

# 3) Impuls p = m * v
m = 0.4  # Masse Wagen in kg (400 g)
p = m * vz

# 3.5 Impuls von Bumper
m_bumper = 1  # Masse Bumper in kg
p_bumper = m_bumper * vz_bumper

# 4) Netto-Kraft und kumulativer Impuls (Integration über die Zeit)
F_net = F_left# - F_right
dt    = np.diff(t, prepend=t[0])
I     = np.cumsum(F_net * dt)  # Impuls durch Kraftstoß ∫F dt

# 5) Plot Impuls vs. kum. Kraftimpuls
plt.figure()
plt.plot(t, p, label='p = m·v')
plt.plot(t, p_bumper, label='p_bumper = m_bumper·v_bumper')
plt.plot(t, I, label='I = ∫F_net dt', linestyle='--')
plt.title('Impuls des Wagens und kumulativer Kraftimpuls (Ohne Feder)')
plt.xlabel('t [s]')
plt.ylabel('Impuls [kg·m/s]')
plt.legend()
plt.grid(True)

plt.show()
