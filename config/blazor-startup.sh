#!/bin/sh

# The file path where Nginx serves the static files
APPSETTINGS="/usr/share/nginx/html/appsettings.json"
NGINX_CONF="/etc/nginx/nginx.conf"

echo "Replacing environment variables in $APPSETTINGS..."

# We use | as a delimiter to avoid issues with slashes in URIs
sed -i "s|<gateway-api-endpoint>|${GATEWAY_API_URI}|g" "$APPSETTINGS"
sed -i "s|<seq-ui-endpoint>|${SEQ_UI_URI}|g" "$APPSETTINGS"
sed -i "s|<jaeger-ui-endpoint>|${JAEGER_UI_URI}|g" "$APPSETTINGS"
sed -i "s|<grafana-dashboard-endpoint>|${GRAFANA_DASHBOARD_URI}|g" "$APPSETTINGS"

echo "Injecting $NGINX_PROXY_AUTH_API_URL into Nginx config..."
sed -i "s|AUTH_API_URL|${NGINX_PROXY_AUTH_API_URL}|g" "$NGINX_CONF"

echo "Configuration complete."
