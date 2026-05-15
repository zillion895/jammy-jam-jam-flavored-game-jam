#include "CityMap.hpp"
#include <iostream>
#include <string>
#include <utility>
#include <vector>

void printResult(const std::string &algorithm,
                 const std::pair<std::vector<std::string>, int> &result);

int main() {
  CityMap city;

  std::cout << "=== George's City Navigation System ===\n\n";

  city.printCity();
  std::cout << "\n";

  std::cout << "--- Route: Downtown -> Airport ---\n\n";
  printResult("greedy", city.greedyPath(1, 2));
  printResult("djikstras", city.dijkstraPath(1, 2));
  printResult("A*", city.aStarPath(1, 2));

  /* Test all three algorithms on this route */

  std::cout << "\n";

  std::cout << "--- Route: Downtown -> Suburb South ---\n\n";
  printResult("greedy", city.greedyPath(0, 7));
  printResult("djikstras", city.dijkstraPath(0, 7));
  printResult("A*", city.aStarPath(0, 7));

  /* Test all three algorithms on this route */

  std::cout << "\n";

  std::cout << "--- Route: Downtown -> Downtown ---\n\n";
  printResult("greedy", city.greedyPath(0, 0));
  printResult("djikstras", city.dijkstraPath(0, 0));
  printResult("A*", city.aStarPath(0, 0));
  /* Pick your own start/end pair and test all three algorithms */

  return 0;
}

void printResult(const std::string &algorithm,
                 const std::pair<std::vector<std::string>, int> &result) {
  std::cout << "[" << algorithm << "]\n";
  if (result.first.empty() || result.second == -1) {
    std::cout << "  No path found.\n";
    return;
  }
  std::cout << "  Path: ";
  for (int i = 0; i < (int)result.first.size(); i++) {
    std::cout << result.first[i];
    if (i < (int)result.first.size() - 1)
      std::cout << " -> ";
  }
  std::cout << "\n  Total time: " << result.second << " min\n";
}
