import { defineStore } from 'pinia';
import { ref } from 'vue';

export const useTabStore = defineStore('tab', () => {
  const settingsFacilitiesActiveTab = ref(0);
  const settingsEncountersActiveTab = ref(0);

  function setFacilitiesActiveTab(index: number) {
    settingsFacilitiesActiveTab.value = index;
  }

  function setEncountersActiveTab(index: number) {
    settingsEncountersActiveTab.value = index;
  }

  return {
    settingsFacilitiesActiveTab,
    settingsEncountersActiveTab,
    setFacilitiesActiveTab,
    setEncountersActiveTab,
  };
});
