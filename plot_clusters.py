import pandas as pd
import matplotlib.pyplot as plt

# Carregar o arquivo CSV gerado pelo C#
data = pd.read_csv("clusters_result.csv")

# Plotar os clusters
plt.figure(figsize=(10, 6))
for cluster in data['Cluster'].unique():
    cluster_data = data[data['Cluster'] == cluster]
    plt.scatter(cluster_data['X'], cluster_data['Y'], label=f'Cluster {cluster}')

plt.xlabel('Componente X')
plt.ylabel('Componente Y')
plt.title('Clusters Resultantes do K-Means')
plt.legend()
plt.show()
