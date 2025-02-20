import { ApolloClient, InMemoryCache, split } from "@apollo/client/core";
import { getMainDefinition } from "@apollo/client/utilities";
import { WebSocketLink } from "@apollo/client/link/ws";
import { SubscriptionClient  } from "subscriptions-transport-ws";
import createUploadLink  from 'apollo-upload-client/createUploadLink.mjs';

// const httpLink = new HttpLink({
//   uri: import.meta.env.VITE_API_URL,
// });

const uploadLink = createUploadLink({
  uri: import.meta.env.VITE_API_URL,
  headers: {
    'GraphQL-Preflight': "true", // Preflight header for multi-part requests
  }
});

const wsLink = new WebSocketLink(
  new SubscriptionClient(import.meta.env.VITE_WS_API_URL, {
  reconnect: true
})
);

const splitLink = split(
  ({ query }) => {
    const definition = getMainDefinition(query);
    return (
      definition.kind === "OperationDefinition" &&
      definition.operation === "subscription"
    );
  },
  wsLink,
  uploadLink,
  // httpLink
);

const client = new ApolloClient({
  link: splitLink,
  cache: new InMemoryCache(),
});

export default client;