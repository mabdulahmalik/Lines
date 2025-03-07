FROM quay.io/phasetwo/phasetwo-keycloak:26.0 AS builder

ENV KC_DB=mssql
ENV KC_METRICS_ENABLED=true
ENV KC_HEALTH_ENABLED=true
ENV KC_FEATURES=persistent-user-sessions,admin-fine-grained-authz,client-secret-rotation,dpop,passkeys,recovery-codes,scripts,token-exchange,update-email


RUN /opt/keycloak/bin/kc.sh --verbose build \
    --spi-email-template-provider=freemarker-plus-mustache \
    --spi-email-template-freemarker-plus-mustache-enabled=true \
    --spi-user-profile-declarative-user-profile-read-only-attributes=org.ro.*

FROM quay.io/keycloak/keycloak:26.0

COPY --from=builder /opt/keycloak/ /opt/keycloak/

COPY ./themes/ /opt/keycloak/themes/
COPY ./providers/ /opt/keycloak/providers/
COPY ./import/ /opt/keycloak/data/import/
#COPY ./cert /cert

ENTRYPOINT ["/opt/keycloak/bin/kc.sh", "start", "--optimized", "--import-realm"]
#ENTRYPOINT ["/opt/keycloak/bin/kc.sh", "start-dev", "--import-realm", "--db=mssql", "--spi-email-template-provider=freemarker-plus-mustache", "--spi-email-template-freemarker-plus-mustache-enabled=true", "--spi-theme-cache-themes=false", "--features=persistent-user-sessions,admin-fine-grained-authz,client-secret-rotation,dpop,passkeys,recovery-codes,scripts,token-exchange,update-email"]
