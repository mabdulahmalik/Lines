<script setup lang="ts">
import { ref, onUnmounted, watch, onMounted } from 'vue';
import { FwbButton } from 'flowbite-vue';
import ModalDrawer from '@/components/modal/ModalDrawer.vue';
import {
  IconExpandWindows,
  IconArrowLeft,
  IconShrinkWindows,
  IconShowSidebar,
  IconHideSidebar,
} from '@/components/icons/index';
import { useModalStore } from '@/stores/modal';
import DrawerHeaderEditName from '@/views/common/DrawerHeaderEditName.vue';
import { FacilityType } from '@/api/__generated__/graphql';
import ViewFacilityModalBody from './ViewFacilityModalBody.vue';

// Props
const props = defineProps<{
  facilityType: FacilityType;
}>();

// Store
const modalStore = useModalStore();

// Facility Name state/medthod
const facilityName = ref('');
onMounted(() => {
  facilityName.value = props.facilityType.name || '';
});
watch(
  () => props.facilityType.name,
  (newValue) => {
    facilityName.value = newValue || '';
  }
);
function changeName(val: string) {
  facilityName.value = val;
}

const viewFacilityModalBodyRef = ref<InstanceType<typeof ViewFacilityModalBody>>();
function handleSaveFacilityType() {
  viewFacilityModalBodyRef.value?.handleSaveFacilityType();
}

// Modal state/functions
const modalViewFacilityTypeRef = ref<InstanceType<typeof ModalDrawer>>();
const setModalOpen = (value: boolean): void => {
  modalViewFacilityTypeRef.value?.setModalOpen(value);
};
function closeFacilityModal() {
  modalViewFacilityTypeRef.value?.setModalOpen(false);
}
const fullWidth = ref(false);
const modalWidth = ref('5xl');
const setModalWidth = (val: string) => {
  modalWidth.value = val;
  if (val === 'full') {
    fullWidth.value = true;
    modalStore.isFacilityTypeModalExpended = true;
  } else {
    fullWidth.value = false;
    modalStore.isFacilityTypeModalExpended = false;
  }
};
onUnmounted(() => {
  modalStore.isFacilityTypeModalExpended = false;
});

// Expose function
defineExpose({
  setModalOpen,
});
</script>
<template>
  <ModalDrawer
    ref="modalViewFacilityTypeRef"
    :z_index="46"
    :max_width="modalWidth"
    hide_header_close_on_mobile
  >
    <template #header>
      <div class="p-4 lg:px-6 flex justify-between min-h-[72px]">
        <div class="flex items-center">
          <!-- Mobile close button -->
          <fwb-button
            @click="closeFacilityModal"
            color="light"
            pill
            square
            size="lg"
            class="bg-white border-white text-slate-400 hover:text-slate-900 lg:hidden p-0 mr-2"
          >
            <IconArrowLeft width="24" height="24" class="text-[#334155]" />
            <span class="sr-only">Close modal</span>
          </fwb-button>

          <DrawerHeaderEditName :name="facilityName" @name-updated="changeName" />
        </div>
        <!-- Icon Buttons Desktop only -->
        <div class="hidden lg:flex items-center gap-2">
          <!-- Expand -->
          <fwb-button
            v-if="!fullWidth"
            @click="setModalWidth('full')"
            color="light"
            pill
            square
            class="bg-white border-white focus:ring-0"
          >
            <IconExpandWindows width="22" height="22" />
          </fwb-button>
          <!-- Shrink -->

          <fwb-button
            v-if="fullWidth"
            @click="setModalWidth('5xl')"
            color="light"
            pill
            square
            class="bg-white border-white focus:ring-0"
          >
            <IconShrinkWindows width="22" height="22" />
          </fwb-button>
          <!-- Hide Sidebar -->
          <fwb-button
            v-if="modalStore.isFacilityTypeSidebarOpen"
            @click="modalStore.isFacilityTypeSidebarOpen = false"
            color="light"
            pill
            square
            class="bg-white border-white focus:ring-0"
          >
            <IconHideSidebar />
          </fwb-button>
          <!-- Show Sidebar -->
          <fwb-button
            v-else
            @click="modalStore.isFacilityTypeSidebarOpen = true"
            color="light"
            pill
            square
            class="bg-white border-white focus:ring-0"
          >
            <IconShowSidebar />
          </fwb-button>
        </div>
      </div>
    </template>
    <template #body>
      <ViewFacilityModalBody
        ref="viewFacilityModalBodyRef"
        :facilityType="props.facilityType"
        :facility-name="facilityName"
        @width="setModalWidth"
      />
    </template>
    <template #footer>
      <div class="p-4 lg:px-6 flex justify-end items-center gap-4 w-full">
        <div class="flex items-center gap-4 lg:gap-6">
          <fwb-button @click="closeFacilityModal" color="light" pill> Cancel</fwb-button>
          <fwb-button @click="handleSaveFacilityType" class="bg-primary-600 hover:bg-brand-600" pill
            >Save</fwb-button
          >
        </div>
      </div>
    </template>
  </ModalDrawer>
</template>
