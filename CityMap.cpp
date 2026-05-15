#include "CityMap.hpp"
#include "Location.hpp"
#include <algorithm>
#include <cmath>
#include <cstddef>
#include <iostream>
#include <limits>
#include <queue>
#include <utility>

CityMap::CityMap() {
  locations.resize(8);

  locations[0].name = "Downtown";
  locations[0].x = 4;
  locations[0].y = 4;
  locations[0].neighbors = {{1, 8}, {3, 15}, {6, 12}};

  locations[1].name = "Harbor";
  locations[1].x = 4;
  locations[1].y = 0;
  locations[1].neighbors = {{0, 8}, {2, 20}, {4, 10}};

  locations[2].name = "Airport";
  locations[2].x = 10;
  locations[2].y = 0;
  locations[2].neighbors = {{1, 20}, {4, 5}, {7, 18}};

  locations[3].name = "University";
  locations[3].x = 0;
  locations[3].y = 6;
  locations[3].neighbors = {{0, 15}, {5, 9}, {6, 7}};

  locations[4].name = "Industrial";
  locations[4].x = 9;
  locations[4].y = 1;
  locations[4].neighbors = {{1, 10}, {2, 5}, {7, 8}};

  locations[5].name = "Medical Center";
  locations[5].x = 2;
  locations[5].y = 9;
  locations[5].neighbors = {{3, 9}, {6, 11}, {7, 14}};

  locations[6].name = "Suburb North";
  locations[6].x = 1;
  locations[6].y = 5;
  locations[6].neighbors = {{0, 12}, {3, 7}, {5, 11}};

  locations[7].name = "Suburb South";
  locations[7].x = 8;
  locations[7].y = 8;
  locations[7].neighbors = {{2, 18}, {4, 8}, {5, 14}};
}

void CityMap::printCity() const {
  std::cout << "City Locations:\n";
  for (int i = 0; i < (int)locations.size(); i++) {
    std::cout << "  [" << i << "] " << locations[i].name << "\n";
    std::cout << "       neighbors: ";
    for (int j = 0; j < (int)locations[i].neighbors.size(); j++) {
      auto [idx, time] = locations[i].neighbors[j];
      std::cout << locations[idx].name << "(" << time << ")";
      if (j < (int)locations[i].neighbors.size() - 1)
        std::cout << ", ";
    }
    std::cout << "\n";
  }
}
// ======================================= my code =============================

std::pair<std::vector<std::string>, int> CityMap::greedyPath(int start,
                                                             int end) {

  int currentNode = start;
  std::pair<std::vector<std::string>, int> path;
  std::vector<bool> visited(locations.size(), false);

  if (start < 0 || end < 0 || start >= (int)locations.size() ||
      end >= (int)locations.size()) {
    return {{}, -1};
  }
  if (start == end) {
    return {{locations[start].name}, 0};
  }
  path.second = 0;
  path.first.push_back(locations[start].name);
  while (currentNode != end) {
    visited[currentNode] = true;
    int best_neighboor = -1;
    int best_travel_time = 0;
    int min = std::numeric_limits<int>::max();

    for (int neighboor = 0; neighboor < locations[currentNode].neighbors.size();
         neighboor++) {
      int neighboorIndex = locations[currentNode].neighbors[neighboor].first;
      int h = heuristic(neighboorIndex, end);
      int travelTime = locations[currentNode].neighbors[neighboor].second;
      if (visited[neighboorIndex])
        continue;
      if (h < min) {
        min = h;
        best_neighboor = neighboorIndex;
        best_travel_time = travelTime;
      }
    }

    currentNode = best_neighboor;
    if (best_neighboor < 0 || best_neighboor >= (int)locations.size()) {
      return {{}, -1};
    }
    // printf("current node: %s", locations[best_neighboor].name.c_str());
    path.first.push_back(locations[best_neighboor].name);
    path.second += best_travel_time;
  }
  return path;
}
std::pair<std::vector<std::string>, int> CityMap::dijkstraPath(int start,
                                                               int end) {
  if (start < 0 || end < 0 || start >= locations.size() ||
      end >= locations.size()) {
    return {{}, -1};
  }
  if (start == end) {
    return {{locations[start].name}, 0};
  }
  std::vector<int> distances(locations.size(), std::numeric_limits<int>::max());
  distances[start] = 0;
  std::vector<int> prev(locations.size(), -1);

  std::priority_queue<std::pair<int, int>, std::vector<std::pair<int, int>>,
                      std::greater<std::pair<int, int>>>
      pq;
  pq.push(std::make_pair(0, start));

  while (pq.size() > 0) {
    int currentDistance = pq.top().first;
    int currentNode = pq.top().second;
    pq.pop();
    if (currentDistance > distances[currentNode]) {
      continue;
    }

    if (currentNode == end) {
      return std::make_pair(reconstructPath(prev, start, end), currentDistance);
    }
    for (auto neighborPair : locations[currentNode].neighbors) {

      int newDistance = distances[currentNode] + neighborPair.second;
      if (newDistance < distances[neighborPair.first]) {

        distances[neighborPair.first] = newDistance;
        prev[neighborPair.first] = currentNode;
        pq.push(std::make_pair(newDistance, neighborPair.first));
      }
    }
  }
  return {{}, -1};
}
std::pair<std::vector<std::string>, int> CityMap::aStarPath(int start,
                                                            int end) {
  if (start < 0 || end < 0 || start >= locations.size() ||
      end >= locations.size()) {
    return {{}, -1};
  }
  if (start == end) {
    return {{locations[start].name}, 0};
  }
  std::vector<int> distances(locations.size(), std::numeric_limits<int>::max());
  distances[start] = 0;
  std::vector<int> prev(locations.size(), -1);

  std::priority_queue<std::pair<int, int>, std::vector<std::pair<int, int>>,
                      std::greater<std::pair<int, int>>>
      pq;
  pq.push(std::make_pair(0, start));

  while (pq.size() > 0) {
    int currentDistance = pq.top().first;
    int currentNode = pq.top().second;
    pq.pop();
    if (currentDistance >
        distances[currentNode] + heuristic(currentNode, end)) {
      continue;
    }

    if (currentNode == end) {
      return std::make_pair(reconstructPath(prev, start, end), currentDistance);
    }
    for (auto neighborPair : locations[currentNode].neighbors) {

      int newDistance = distances[currentNode] + neighborPair.second;
      if (newDistance < distances[neighborPair.first]) {

        distances[neighborPair.first] = newDistance;
        prev[neighborPair.first] = currentNode;
        pq.push(std::make_pair(newDistance + heuristic(neighborPair.first, end),
                               neighborPair.first));
      }
    }
  }
  return {{}, -1};
}
std::vector<std::string> CityMap::reconstructPath(const std::vector<int> &prev,
                                                  int start, int end) const {
  std::vector<int> rev;
  int cur = end;
  while (cur != -1) {
    rev.push_back(cur);
    if (cur == start)
      break;
    cur = prev[cur];
  }

  if (rev.empty() || rev.back() != start) {
    return {}; // no path
  }

  std::reverse(rev.begin(), rev.end());
  std::vector<std::string> path;
  path.reserve(rev.size());
  for (int idx : rev)
    path.push_back(locations[idx].name);
  return path;
}
