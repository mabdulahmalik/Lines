import { ref } from 'vue';
import { defineStore } from 'pinia';
import client from '@/api';
import { provideApolloClient } from '@vue/apollo-composable';
import { State, useGetStatesQuery } from '@/api/__generated__/graphql';

export const useStatesStore = defineStore('states', () => {
  provideApolloClient(client);

  const states = ref<State[]>([]);

  // Fetch States
  const { onResult: getStatesResult } = useGetStatesQuery();
  getStatesResult((result: any) => {
    states.value = result?.data?.states || [];
  });

  const getStateFullName = (stateCode: string) => {
    const state = states.value.find((s) => s.code === stateCode);
    return state ? state.name : stateCode;
  };

  return {
    states,
    getStateFullName,
  };
});
