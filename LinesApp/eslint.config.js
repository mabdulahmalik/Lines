import globals from "globals";
import pluginJs from "@eslint/js";
import tseslint from "typescript-eslint";
import pluginVue from "eslint-plugin-vue";
import prettier from "eslint-plugin-prettier";
import prettierConfig from "eslint-config-prettier";

/** @type {import('eslint').Linter.Config[]} */
export default [
  // Base configuration
  {
    ignores: [
      '**/node_modules/**',
      'dist/**',
      'build/**',
      'coverage/**',
      '.cache/**',
      '*.log',
      '.env*'
    ],
  },
  {
    files: ["**/*.{js,mjs,cjs,ts,vue}"],
    plugins: {
      'prettier': prettier
    },
    languageOptions: { 
      globals: globals.browser,
      parserOptions: {
        ecmaVersion: 'latest',
        sourceType: 'module'
      }
    },
  },
  // Extend configurations
  pluginJs.configs.recommended,
  ...tseslint.configs.recommended,
  ...pluginVue.configs["flat/essential"],
  // Vue specific configuration
  {
    files: ["**/*.vue"], 
    languageOptions: {
      parserOptions: {
        parser: tseslint.parser
      }
    }
  },
  // Prettier configuration
  {
    files: ["**/*.{js,ts,vue}"], 
    rules: {
      "prettier/prettier": "error"
    }
  },
  prettierConfig
];