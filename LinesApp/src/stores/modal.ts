import { defineStore } from 'pinia';
import { ref } from 'vue';

export const useModalStore = defineStore('modal', () => {
  const isModalDrawerExpended = ref(false);

  const isRecordModalExpended = ref(false);
  const isRecordModalAutoExpended = ref(false);

  const isLineModalExpended = ref(false);
  const isLineModalAutoExpended = ref(false);

  const isJobModalExpended = ref(false);
  const isJobModalAutoExpended = ref(false);

  const isFacilitiesModalExpended = ref(false);
  const isFacilitiesModalAutoExpended = ref(false);

  const isFacilityTypeModalExpended = ref(false);
  const isFacilityTypeModalAutoExpended = ref(false);
  const isFacilityTypeSidebarOpen = ref(true);
  const isRoomPropertySidebarOpen = ref(true);

  const isEncounterActitityOpen = ref(true);
  const isEncounterSidebarOpen = ref(true);

  const isLineSidebarOpen = ref(true);
  const isRecordSidebarOpen = ref(true);

  // Inner Model placment outside the the component
  const isGlobalModalOpen = ref(false);
  const globalModal = ref(null);

  function openGlobalModal(modal: any) {
    isGlobalModalOpen.value = true;
    globalModal.value = modal;
  }
  function closeGlobalModal() {
    isGlobalModalOpen.value = false;
    globalModal.value = null;
  }

  return {
    isModalDrawerExpended,
    isRecordModalExpended,
    isRecordModalAutoExpended,
    isLineModalExpended,
    isLineModalAutoExpended,
    isJobModalExpended,
    isJobModalAutoExpended,
    isFacilitiesModalExpended,
    isFacilitiesModalAutoExpended,
    isFacilityTypeModalExpended,
    isFacilityTypeModalAutoExpended,
    isFacilityTypeSidebarOpen,
    isRoomPropertySidebarOpen,
    globalModal,
    isGlobalModalOpen,
    openGlobalModal,
    closeGlobalModal,
    isEncounterActitityOpen,
    isEncounterSidebarOpen,
    isLineSidebarOpen,
    isRecordSidebarOpen,
  };
});
