@ECHO OFF
ECHO Start Consul
cd consul
consul.exe agent -dev -enable-script-checks -config-dir=./config
PAUSE
