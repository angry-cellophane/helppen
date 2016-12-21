#! /bin/bash -e

function build_infra {
  echo 'Creating folders structure ...'
  local work_dir="$1"
  rm -rf "$work_dir/.build"
  mkdir "$work_dir/.build"

  local root_dir="$work_dir/.build/tasks"
  mkdir "$root_dir"

  echo 'Folders structure created'
}

function build_app {
  echo 'Building app ...'
  local work_dir="$1"
  local server_dir="$work_dir/.build/tasks/server"

  mkdir "$server_dir"

  cp -r "$work_dir/node_modules" "$server_dir/node_modules"
  mkdir "$server_dir/src"
  cp -r "$work_dir/src/app" "$server_dir/src/app"

  cp "$work_dir/src/infra/server/"* "$server_dir/"
  echo 'App has been build'
}

function build_deployment {
  local work_dir="$1"
  local dep_dir="$work_dir/.build/tasks/deployment"

  mkdir "$dep_dir"

  cp -r "$work_dir/src/database" "$dep_dir"
  cp -r "$work_dir/src/infra/deployment/"* "$dep_dir"
}

function pack {
  echo 'Packing archive ...'
  local work_dir="$1"
  old_pwd="$(pwd)"
  cd "$work_dir/.build/"
  tar cfz tasks.tar.gz ./tasks/

  pwd "$old_pwd"
  echo "The project packed into $work_dir/.build/tasks.tar.gz"
}

work_dir="$(dirname ${BASH_SOURCE[0]})"

build_infra "$work_dir"
build_app "$work_dir"
build_deployment "$work_dir"
pack "$work_dir"

echo 'Finished successfully'

