<script setup lang="ts">
import ModalDrawer from '@/components/modal/ModalDrawer.vue';
import { useModalStore } from '@/stores/modal';
import { ref, onUnmounted, onMounted, watch } from 'vue';
import { FwbButton } from 'flowbite-vue';
import RoutinesModalBody from './RoutinesModalBody.vue';
import {
  IconExpand01,
  IconArrowLeft,
  IconShrink,
} from '@/components/icons/index';
import DrawerHeaderEditName from '@/views/common/DrawerHeaderEditName.vue';

const props = defineProps<{
  name: string;
  description: string;
  followUp: boolean;
}>();

const modalStore = useModalStore();

// Routine Name
const routineName = ref('');
const routineDescription = ref('');
const followUp = ref(false)
onMounted(() => {
  routineName.value = props.name;
  routineDescription.value = props.description;
  followUp.value = props.followUp;
});

watch(
  () => props.name,
  (newValue) => {
    routineName.value = newValue;
  }
);

watch(
  () => props.description,
  (newValue) => {
    routineDescription.value = newValue;
  }
);

watch(
  () => props.followUp,
  (newValue) => {
    followUp.value = newValue;
  }
);
function changeName(val: string) {
  routineName.value = val;
}
// submit forms data on save
const RoutineModalBodyRef = ref<InstanceType<typeof RoutinesModalBody>>();
const handleFooterSave = () => {
  RoutineModalBodyRef.value?.submittedData();
};

function closeRoutineModalDrawer() {
  careateRoutineModalDrawerRef.value?.setModalOpen(false);
}

// Drawer states
const careateRoutineModalDrawerRef = ref<InstanceType<typeof ModalDrawer>>();
const setModalOpen = (value: boolean): void => {
  careateRoutineModalDrawerRef.value?.setModalOpen(value);
};
const fullWidth = ref(false);
const modalWidth = ref('5xl');
const setModalWidth = (val: string) => {
  modalWidth.value = val;
  if (val === 'full') {
    fullWidth.value = true;
    modalStore.isModalDrawerExpended = true;
  } else {
    fullWidth.value = false;
    modalStore.isModalDrawerExpended = false;
  }
};
const onModalDrawerClosed = () => {
  routineName.value = '';
  routineDescription.value = '';
};

const isSaveEnabled = ref(false);
function handleComplete(val: boolean) {
  isSaveEnabled.value = val;
}
onUnmounted(() => {
  modalStore.isModalDrawerExpended = false;
});
defineExpose({
  setModalOpen,
});
</script>
<template>
  <!-- routine modal drawer -->
  <ModalDrawer
    ref="careateRoutineModalDrawerRef"
    :z_index="46"
    :max_width="modalWidth"
    hide_header_close_on_mobile
    @close="onModalDrawerClosed"
  >
    <template #header>
      <!-- Mobile status badge -->
      <div
        class="lg:hidden w-100 p-2 font-semibold flex justify-center items-center text-center capitalize"
        :class="true ? 'bg-[#FCE8F3] text-[#99154B]' : 'text-blue-700 bg-blue-100'"
      >
        Inactive
      </div>

      <div
        v-if="followUp"
        class="lg:hidden w-100 p-2 font-semibold flex justify-center items-center text-center text-green-700 bg-green-100"
      >
        Follow up
      </div>

      <div class="p-4 lg:px-6 flex justify-between min-h-[72px]">
        <div class="flex items-center">
          <!-- Mobile close button -->
          <fwb-button
            @click="closeRoutineModalDrawer"
            color="light"
            pill
            square
            size="lg"
            class="bg-white border-white text-slate-400 hover:text-slate-900 lg:hidden p-0 mr-2"
          >
            <IconArrowLeft width="24" height="24" class="text-[#334155]" />
            <span class="sr-only">Close modal</span>
          </fwb-button>

          <!-- Name and edit name from -->
          <DrawerHeaderEditName :name="routineName" @name-updated="changeName" />
        </div>

        <!-- Icon Buttons Desktop only -->
        <div class="hidden lg:flex items-center gap-2 lg:gap-4">
          <!-- status -->
          <span
            class="text-xs leading-[18px] font-medium rounded-full py-0.5 px-2.5 capitalize"
            :class="true ? 'bg-[#FCE8F3] text-[#99154B]' : 'text-blue-700 bg-blue-100'"
            >Inactive</span
          >
          <!-- is follow up -->
          <span
            v-if="followUp"
            class="text-xs leading-[18px] font-medium rounded-full py-0.5 px-2.5 bg-green-100 text-green-700"
            >Follow up</span>
          <!-- Expand -->
          <fwb-button v-if="!fullWidth" @click="setModalWidth('full')" color="light" pill square>
            <IconExpand01 />
          </fwb-button>
          <!-- Shrink -->
          <fwb-button v-if="fullWidth" @click="setModalWidth('5xl')" color="light" pill square>
            <IconShrink />
          </fwb-button>
        </div>
      </div>
    </template>
    <template #body class="bg-red-200">
      <RoutinesModalBody
        ref="RoutineModalBodyRef"
        :is-create="true"
        :name="routineName"
        :description="routineDescription"
        :follow-up="followUp"
        @close="setModalOpen(false)"
        @complete="handleComplete"
      />
    </template>
    <template #footer>
      <div class="p-4 lg:px-6 flex justify-end items-center gap-4 w-full">
        <fwb-button @click="closeRoutineModalDrawer" color="light" pill> Cancel</fwb-button>
        <fwb-button
          @click="handleFooterSave"
          class="bg-primary-600 hover:bg-brand-600"
          pill
          :disabled="!isSaveEnabled"
          >Save</fwb-button
        >
      </div>
    </template>
  </ModalDrawer>
</template>
