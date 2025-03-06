FROM quay.io/phasetwo/phasetwo-keycloak:26.0 AS builder

RUN /opt/keycloak/bin/kc.sh --verbose build \
    --db=mssql \
    --metrics-enabled=true \
    --health-enabled=true \
    --features=persistent-user-sessions,admin-fine-grained-authz,client-secret-rotation,dpop,passkeys,recovery-codes,scripts,token-exchange,update-email \
    --spi-email-template-provider=freemarker-plus-mustache \
    --spi-email-template-freemarker-plus-mustache-enabled=true \
    --spi-theme-cache-themes=false \
    --spi-user-profile-declarative-user-profile-read-only-attributes=org.ro.*

FROM quay.io/keycloak/keycloak:26.0

ENV KC_HTTP_MANAGEMENT_ENABLED=true
ENV KC_HTTP_MANAGEMENT_PORT=9000
ENV KC_HOSTNAME=salubrity.app
ENV KC_HOSTNAME_STRICT=true
ENV KC_HTTP_ENABLED=true
ENV KC_HOSTNAME_STRICT_HTTPS=false

COPY --from=builder /opt/keycloak/ /opt/keycloak/

COPY ./themes/ /opt/keycloak/themes/
COPY ./providers/ /opt/keycloak/providers/
COPY ./import/ /opt/keycloak/data/import/
#COPY ./cert /cert

ENTRYPOINT ["/opt/keycloak/bin/kc.sh", "start", "--optimized", "--import-realm"]