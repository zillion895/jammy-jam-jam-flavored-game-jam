#pragma once

#include <string>
#include <utility>
#include <vector>

// DO NOT MODIFY THIS FILE
// This struct is used by the auto-grader exactly as written.

struct Location {
  std::string name;
  int x;
  int y;
  std::vector<std::pair<int, int>> neighbors;
  // neighbors[i] = {locationIndex, travelTime}
};
