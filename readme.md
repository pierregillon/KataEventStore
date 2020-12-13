# Readme

## Event store from Docker
```
docker run --name esdb-node -it -p 2113:2113 -p 1113:1113 \
    eventstore/eventstore:latest --insecure --run-projections=All \
    --enable-external-tcp --enable-atom-pub-over-http
```

## Kata Step

### TransactionDomain
```
git checkout steps/1-TransactionDomain
```
### TransactionPresentation
```
git checkout steps/2-TransactionPresentation
```
- In memory projection that is regenerated at process startup
- Subscribed to ALL events of the Eventstore (projections, configuration, ...)

### TransactionPresentation.Persisted
```
git checkout steps/3-TransactionPresentationPersisted
```

You need to enable the $by_category projection on EventStore here http://127.0.0.1:2113/web/index.html#/projections

- Example using a persistent projection
- The EventStore remembers the last ack event and send only new ones that match the subscribed stream

## EventStore vs Bus only
- Can replay all events from the begining
- Can create custom projection to replay only a part of the events that occured (and concerns a specific app)


But : single point of failure