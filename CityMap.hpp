#pragma once

#include "Location.hpp"
#include <cmath>
#include <string>
#include <utility>
#include <vector>

class CityMap {
public:
  CityMap();

  void printCity() const;

  // Returns {path, totalCost}
  // path = vector of location names from start to end
  // totalCost = total travel time in minutes
  // If no path exists, return {{}, -1}
  std::pair<std::vector<std::string>, int> greedyPath(int start, int end);
  std::pair<std::vector<std::string>, int> dijkstraPath(int start, int end);
  std::pair<std::vector<std::string>, int> aStarPath(int start, int end);

private:
  std::vector<Location> locations;

  // Returns Euclidean distance between two locations' coordinates, truncated to
  // int Used as the admissible heuristic for A*
  int heuristic(int from, int to) const {
    int x1 = locations[from].x;
    int y1 = locations[from].y;

    int x2 = locations[to].x;
    int y2 = locations[to].y;
    return std::sqrt(std::pow(x2 - x1, 2) + std::pow(y2 - y1, 2));
  }

  // Traces back through prev[] to build a vector of location names
  // Returns empty vector if no path exists
  std::vector<std::string> reconstructPath(const std::vector<int> &prev,
                                           int start, int end) const;
};
