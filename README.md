## Auto-instrumentation on Kubernetes

To enable auto-instrumentation for deployments running Kuernetes: use the [OpenTelemetry Operator](https://opentelemetry.io/docs/kubernetes/operator/automatic/)

Auto-instrumentation on Kubernetes is currently availible for NET, Java, Node.js, Python, and Go.


## Generelt

Start tidlig i utviklingsprosessen med å legge til OpenTelemetry. Det er en god del boilerplate som skal legges til.

Hvordan instrumentere en .NET applikasjon:
https://opentelemetry.io/docs/languages/net/instrumentation/

Hvordan rigge instrumentert applikasjon til å snakke med OTEL Collector:
https://opentelemetry.io/docs/languages/net/exporters/
