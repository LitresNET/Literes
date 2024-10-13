import globals from "globals";
import pluginReact from "eslint-plugin-react";


export default [
  {files: ["**/*.{js,mjs,cjs,jsx}"]},
  {languageOptions: { globals: globals.browser }},
  {rules: {
   "react/jsx-filename-extension": [1, { "extensions": [".js", ".jsx"] }],
  }},
  pluginReact.configs.flat['jsx-runtime'],
];