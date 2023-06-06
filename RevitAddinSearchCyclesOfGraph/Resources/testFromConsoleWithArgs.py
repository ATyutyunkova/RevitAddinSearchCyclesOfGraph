import sys
import networkx as nx
import json
import ast
import time


def writeToLogs(time, text):
    with open("C:\\Users\\tyutyunkova\\Desktop\\IDK\\tempGraphFolder\\logs.txt", "a") as file:
        file.write(f'{time:.2f}' + text)
        file.write("\n")


def writeToLogsLine(text):
    with open("C:\\Users\\tyutyunkova\\Desktop\\IDK\\tempGraphFolder\\logs.txt", "a") as file:
        file.write(text)
        file.write("\n")


class GraphCyclesResultDto:
    cycles = []


path = sys.argv[1]

start_read_data = time.perf_counter()
with open(path, 'r') as fp:
    data = fp.read()
writeToLogs(time.perf_counter() - start_read_data, " - чтение файла")

start_deserialization = time.perf_counter()
graph = ast.literal_eval(data)
writeToLogs(time.perf_counter() - start_deserialization, " - десериализация из строки в объект")

start_create_graph = time.perf_counter()
G = nx.Graph()
for edge in graph:
    v1 = edge['Item1']
    v2 = edge['Item2']
    G.add_node(v1)
    G.add_node(v2)
    G.add_edge(v1, v2)

writeToLogs(time.perf_counter() - start_create_graph, " - создание графа")

start_find_cycles = time.perf_counter()
graphCyclesResultDto = GraphCyclesResultDto()

for cycle in nx.cycle_basis(G):
    graphCyclesResultDto.cycles += [cycle]

writeToLogs(time.perf_counter() - start_find_cycles, " - нахождение циклов в графе")

start_to_json = time.perf_counter()
jsonStr = json.dumps(graphCyclesResultDto.__dict__)
writeToLogs(time.perf_counter() - start_to_json, " - преобразование в json")

lengthOfVertex = len(graphCyclesResultDto.cycles) + 1

writeToLogsLine("Кол-во вершин: " + str(lengthOfVertex))
writeToLogsLine("--------------------end-----------------------")

print(jsonStr)
