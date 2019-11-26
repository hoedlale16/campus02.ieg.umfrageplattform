# campus02.ieg.umfrageplattform
Das Projekt zur Lehrveranstaltung IEG (Integration elektronischer Geschäftsprozesse) am Campus02 - Masterstudiengang IT &amp; Wirtschaftsinformatik


# Setup Consul
- HashiCorp Conusl must be downloaded from https://www.consul.io/downloads.html
- consul.exe must be stored in consul-directory 
- Required configuration for consul is located in directory consul/config
- Start consul using command:
>switch with CMD-Line to consul directory
>./consul.exe agent -dev -enable-script-checks -config-dir=./config

Consul is reachable at port 8500 (http://localhost:8500)
