<script setup lang="ts">
import { FwbButton, FwbTooltip, FwbInput } from 'flowbite-vue';
import MoreDropdown from '../../partials/MoreDropdown.vue';
import { useModalStore } from '@/stores/modal';
import {
  IconArrowLeft,
  IconExpandWindows,
  IconShrinkWindows,
  IconShowSidebar,
  IconHideSidebar,
  IconKeyframeAlignHorizontal,
  IconLogin01,
  IconAddCircleHalfDot,
  IconEdit03,
} from '@/components/icons/index';
import { ref, watch, onMounted, computed } from 'vue';
import { useForm, useField } from 'vee-validate';
import * as yup from 'yup';
import { Line } from '@/api/__generated__/graphql';
import { useEncountersStore } from '@/stores/data/encounters';
import { useProceduresStore } from '@/stores/data/settings/procedures';
import SkeletonItem from '@/components/skeletons/SkeletonItem.vue';
import { useLoaders } from '@/hooks/useLoaders';


const props = defineProps<{
  line: Line;
}>();
const emit = defineEmits<{
  (e: 'close'): void;
  (e: 'width', val: string): void;
  (e: 'lineName', val: string): void;
}>();
const { isLineLoading: isLoading } = useLoaders();
const modalStore = useModalStore();
const fullWidth = ref(false);
const encountersStore = useEncountersStore();
const proceduresStore = useProceduresStore();

const closeModal = () => {
  emit('close');
};

const setModalWidth = (val: string) => {
  emit('width', val);
  if (val === 'full') {
    fullWidth.value = true;
    modalStore.isLineModalExpended = true;
  } else {
    fullWidth.value = false;
    modalStore.isLineModalExpended = false;
  }
};

// Valitiation
const inputErrorClasses = ref('bg-radical-red-50 border-radical-red-500');
const schemaEditLineName = yup.object({
  lineNameEdit: yup.string().required('This is a required field'),
});
const { handleSubmit: handleSubmittedLineName } = useForm({
  validationSchema: schemaEditLineName,
});

// Line Name
const lineName = ref(props.line.name);
onMounted(() => {
  lineName.value = props.line.name!;
});
watch(
  () => props.line.name,
  (newValue) => {
    lineName.value = newValue!;
  }
);
const { value: lineNameEdit, errorMessage: lineNameEditError } = useField<string>('lineNameEdit');
const isEditLineName = ref(false);
const handleClickLineNameEditIcon = () => {
  lineNameEdit.value = lineName.value!;
  isEditLineName.value = true;
};
const cancelEditLineName = () => {
  isEditLineName.value = false;
};

const saveEditLineName = () => {
  handleSubmittedLineName(async () => {
    lineName.value = lineNameEdit.value;
    isEditLineName.value = false;
    emit('lineName', lineName.value);
  })();
};

// badges
const formattedDwelling = computed(() => {
  if (!props.line?.dwelling) return '';
  return props.line.dwelling.charAt(0).toUpperCase() + props.line.dwelling.slice(1).toLowerCase();
});

// Procedure Name
const procedureName = computed(() => {
  const encounter = encountersStore.encounters.find(
    (encounter) => encounter.id === props.line.insertedWith?.encounterId
  );
  const procedureEn = encounter?.procedures?.find(
    (procedure) => procedure?.id === props.line.insertedWith?.encounterProcedureId
  );
  const procedure = proceduresStore.procedures.find(
    (procedure) => procedure?.id === procedureEn?.procedureId
  );
  return procedure?.name || null;
});
</script>

<template>
<div>
  <!-- loading -->
  <div v-if="isLoading" class="flex items-center justify-between gap-x-2 py-2 px-4 lg:px-6 lg:gap-x-4 flex-wrap">
    <div class="flex items-center gap-2  w-full lg:w-auto lg:justify-none min-h-10 justify-start">
    <!-- Mobile close button -->
    <fwb-button
    @click="closeModal"
    color="light"
    pill
    square
    size="lg"
    class="bg-white border-white text-slate-400 hover:text-slate-900 lg:hidden p-0 mr-2"
    >
    <IconArrowLeft width="24" height="24" class="text-[#334155] -ml-1" />
    </fwb-button>
    <SkeletonItem backgroundColor="#CBD5E1" class=" sm:w-56 w-40 h-4 rounded-3xl" />
    </div>
  <hr class="lg:hidden h-[1px] border-t border-slate-300 pt-2 mt-1.5 -mx-4 w-[calc(100%+32px)]" />
    <div class="flex items-center  w-full lg:w-auto justify-end flex-1 min-h-10 gap-4">
    <SkeletonItem backgroundColor="#CBD5E1" class="w-6 h-6 rounded-full" />
    <SkeletonItem backgroundColor="#CBD5E1" class="w-6 h-6 rounded-full" />
    <SkeletonItem backgroundColor="#CBD5E1" class="w-6 h-6 rounded-full" />
    <SkeletonItem backgroundColor="#CBD5E1" class="w-6 h-6 rounded-full" />
    </div>
  </div>

  <div v-else class="flex items-center justify-between gap-x-2 py-2 px-4 lg:px-6 lg:gap-x-4 flex-wrap">
    <!-- Title -->
    <div class="flex items-center gap-2 justify-between w-full lg:w-auto lg:justify-none min-h-10">
      <div class="flex items-center">
        <!-- Mobile close button -->
        <fwb-button
          @click="closeModal"
          color="light"
          pill
          square
          size="lg"
          class="bg-white border-white text-slate-400 hover:text-slate-900 lg:hidden p-0 mr-2"
        >
          <IconArrowLeft width="24" height="24" class="text-[#334155] -ml-1" />
          <span class="sr-only">Close modal</span>
        </fwb-button>
        <!-- Edit line name -->
        <div v-if="isEditLineName" class="flex items-center gap-3">
          <fwb-input
            v-model.trim="lineNameEdit"
            size="sm"
            placeholder="Write text here"
            class="sm:w-[295px]"
            :class="lineNameEditError ? inputErrorClasses : ''"
          />
          <div class="flex items-center gap-3">
            <fwb-button @click="cancelEditLineName" size="sm" color="light" pill>
              Cancel</fwb-button
            >
            <fwb-button
              @click="saveEditLineName"
              size="sm"
              class="bg-primary-600 hover:bg-brand-600"
              pill
              >Confirm</fwb-button
            >
          </div>
        </div>

        <div v-else class="flex items-center gap-3">
          <h2 class="text-lg lg:text-2xl font-semibold">
            {{ lineName }}
          </h2>
          <fwb-button
            @click.stop="handleClickLineNameEditIcon"
            color="light"
            pill
            square
            class="border-transparent"
          >
            <IconEdit03 />
          </fwb-button>
        </div>
        
      </div>
    </div>
    <hr class="lg:hidden h-[1px] border-t border-slate-300 pt-2 mt-1.5 -mx-4 w-[calc(100%+32px)]" />

    <div class="flex items-center gap-3 lg:gap-4 w-full lg:w-auto justify-between flex-1">
      <!-- badges -->
      <div class="flex items-center lg:gap-4 gap-2 lg:mx-6">
        <span
          v-if="procedureName"
          class="flex items-center justify-center gap-1 text-sm font-medium py-0.5 px-3 rounded-md bg-slate-100 text-slate-900"
        >
          <IconKeyframeAlignHorizontal />
          <span>
            {{ procedureName }}
          </span>
        </span>
        <span
          v-if="props.line?.externallyPlaced"
          class="flex items-center justify-center gap-1 text-sm font-medium py-0.5 px-3 rounded-md bg-[#FDF6B2] text-[#8E4B10]"
        >
          <IconLogin01 />
          <span> EXT. </span>
        </span>
        <span
          class="flex items-center justify-center gap-1 text-sm font-medium py-0.5 px-3 rounded-md bg-[#E1EFFE] text-[#1A56DB]"
        >
          <IconAddCircleHalfDot />
          <span>
            {{ formattedDwelling }}
          </span>
        </span>
      </div>
      <!-- Icon Buttons -->
      <div class="flex items-center gap-1 lg:gap-2">
        <!-- More dropdown  -->
        <MoreDropdown :line="props.line" />
        <!-- Expand -->
        <fwb-tooltip v-if="!fullWidth" placement="bottom" class="hidden lg:block">
          <template #trigger>
            <fwb-button
              @click="setModalWidth('full')"
              color="light"
              pill
              square
              class="bg-white border-white focus:ring-0"
            >
              <IconExpandWindows width="24" height="24" />
            </fwb-button>
          </template>
          <template #content> Expand windows </template>
        </fwb-tooltip>

        <!-- Shrink -->
        <fwb-tooltip v-if="fullWidth" placement="bottom" class="hidden lg:block">
          <template #trigger>
            <fwb-button
              @click="setModalWidth('7xl')"
              color="light"
              pill
              square
              class="bg-white border-white focus:ring-0"
            >
              <IconShrinkWindows width="24" height="24" />
            </fwb-button>
          </template>
          <template #content> Shrink windows </template>
        </fwb-tooltip>

        <!-- Hide Sidebar -->
        <fwb-tooltip placement="bottom" v-if="modalStore.isLineSidebarOpen">
          <template #trigger>
            <fwb-button
              @click="modalStore.isLineSidebarOpen = false"
              color="light"
              pill
              square
              class="bg-white border-white focus:ring-0"
            >
              <IconHideSidebar />
            </fwb-button>
          </template>
          <template #content> Hide Sidebar </template>
        </fwb-tooltip>

        <!-- Show Sidebar -->
        <fwb-tooltip placement="bottom" v-else>
          <template #trigger>
            <fwb-button
              @click="modalStore.isLineSidebarOpen = true"
              color="light"
              pill
              square
              class="bg-white border-white focus:ring-0"
            >
              <IconShowSidebar />
            </fwb-button>
          </template>
          <template #content> Show Sidebar </template>
        </fwb-tooltip>
      </div>
    </div>
  </div>
</div>
</template>
