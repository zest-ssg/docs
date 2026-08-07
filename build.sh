#!/usr/bin/env bash
curl -sSL https://dot.net/v1/dotnet-install.sh > dn.sh
chmod +x dn.sh
./dn.sh --channel 10.0 --install-dir $HOME/.dotnet
export PATH="$HOME/.dotnet:$HOME/.dotnet/tools:$PATH"
export DOTNET_ROOT="$HOME/.dotnet"
export DOTNET_ROOT_X64="$HOME/.dotnet"
hash -r
dotnet tool install --global zest
zest build
