import networkx as nx
import time

start_time_create_graph = time.perf_counter()
G = nx.Graph()

countOfVertex = range(1000001)

for vertex in countOfVertex:
    G.add_node(vertex)
    if vertex != 0:
        lastV = vertex - 1
        G.add_edge(lastV, vertex)
        if vertex == countOfVertex[vertex]:
            G.add_edge(vertex, countOfVertex[0])
        else:
            continue
    else:
        continue
start_time_find_cycle = time.perf_counter()
print(time.perf_counter() - start_time_create_graph, "seconds in creation graph")


print(len(nx.cycle_basis(G)))

print(time.perf_counter() - start_time_find_cycle, "seconds in find cycles")

# for cycle in nx.cycle_basis(G):
#     print(cycle)

