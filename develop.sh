#!/bin/bash

# This script watches for changes to certain files and triggers a rebuild
# This is to allow for hot-reloading with dotnet and React together

# Vars
hash=""
nodePID=0
exit=""
declare -A pids # Store PIDs of background processes

# Exit behavior
trap "exit" INT TERM
trap "kill 0" EXIT

# Functions
getHash() {
  echo $(ls -lR ./Backend/Contact/Contact.cs | sha1sum)
}

run_commands() {
  make develop-dotnet & pids["dotnet"]=$!
}

kill_commands() {
  for name in "${!pids[@]}"; do
    echo "Killing $name process (PID: ${pids[$name]})"
    kill "${pids[$name]}"
  done
}

echo "Starting"
make develop-node & nodePID=$!
run_commands
hash=$(getHash)

echo "Press any key to exit"
until [ "$exit" = "yes" ]; do
    newHash=$(getHash)
    if [ "$hash" != "$newHash" ]; then
        kill_commands
        hash=$(getHash)
        echo "New Hash Detected Rebuilding"
        run_commands
    fi
    read -n1 -t 2 && exit="yes"
done

kill_commands
kill "$nodePID"

echo "Exiting"