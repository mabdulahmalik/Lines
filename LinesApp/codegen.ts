import type { CodegenConfig } from '@graphql-codegen/cli';

const config: CodegenConfig = {
  overwrite: true,
  //schema: process.env.GQL_SCHEMA_PATH,
  schema: "./schema.graphql",
  documents: "src/**/*.graphql",
  ignoreNoDocuments: true,
  generates: {
    "src/api/__generated__/graphql.ts": {
      // preset: "client",
      plugins: [
        'typescript',
        'typescript-operations',
        'typescript-vue-apollo',
        {
          add: {
            content: '// @ts-nocheck',
          },
        }
      ],
      config: {
        withCompositionFunctions: true,
        avoidOptionals: true, // Prevents optional fields that could lead to duplicates,
        vueCompositionApiImportFrom: 'vue',
        enumsAsTypes: false,
        vueApolloComposableImportFrom: '@vue/apollo-composable',
        addUnderscoreToArgsType: true,
        skipTypename: true, // Optionally skip __typename from being generated
      },
    },
    // "./graphql.schema.json": {
    //   plugins: ["introspection"]
    // }
  }
};

export default config;
