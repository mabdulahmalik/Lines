<script setup lang="ts">
import { FwbButton, FwbToggle } from 'flowbite-vue';
import { IconSettings02 } from '@/components/icons/index';
import { BaseTable, THead, TBody, TR, TD, TH } from '@/components/table/index';
import { computed, ref, watch } from 'vue';
import { usePurposesStore } from '@/stores/data/settings/purposes';
import { Facility } from '@/api/__generated__/graphql';
import { useRouter } from 'vue-router';
import { useSidebarStore } from '@/stores/sidebar';

const props = defineProps<{
  facility: Facility;
}>();

const emit = defineEmits(['updateExcludedPurposeIds', 'closeModal']);
const sidebarStore = useSidebarStore();
const router = useRouter();
const purposesStore = usePurposesStore();
const purposes = computed(() => purposesStore.purposes);

const activePurposes = computed(() => purposes.value.filter((purpose) => purpose.archived === false));

// Toggles
const toggledPurposes = ref<{ [key: string]: boolean }>({});

// Initialize the toggled state: all on except those in props.facility.purposeIds (which are off)
purposes.value.forEach((purpose) => {
  toggledPurposes.value[purpose.id] = !(props.facility.purposeIds?.includes(purpose.id) ?? false);
});

watch(
  toggledPurposes,
  (newVal) => {
    const excludedPurposeIds = Object.keys(newVal).filter((id) => !newVal[id]);
    emit('updateExcludedPurposeIds', excludedPurposeIds);
    // console.log('Excluded Purposes:', excludedPurposeIds);
  },
  { deep: true }
);

// navigate To Settings
const navigateToSettings = () => {
  emit('closeModal', true);
  router.push({ path: '/settings/encounters' });
  sidebarStore.page='settings';
  sidebarStore.selected='Encounters';
};
</script>

<template>
  <!-- Purposes -->
  <div>
    <div>
      <div class="font-semibold text-lg">Purposes</div>
      <div class="font-medium text-sm pt-1">
        The Purposes listed below are indicative of the types of services that can be provided at this facility. 
        Toggle the switch to exclude a purpose from the facility and prevent it from being selected for a Job.
      </div>
    </div>

    <!-- Settings Button -->
    <div class="flex justify-end mt-4 py-[14px]">
      <fwb-button @click="navigateToSettings" color="light" pill :disabled="facility?.archived??false" class="font-semibold inline-flex">
        <template #prefix>
          <IconSettings02 />
        </template>
        Go to settings
      </fwb-button>
    </div>

    <!-- Table -->
    <div class="overflow-x-auto">
      <base-table>
        <t-head class="uppercase">
          <t-r>
            <t-h>Name</t-h>
            <t-h></t-h>
          </t-r>
        </t-head>
        <t-body v-if="purposes.length > 0">
          <t-r v-for="item in activePurposes" :key="item.id" class="hover:bg-slate-50">
            <t-d
              :class="[
                'font-semibold',
                toggledPurposes[item.id] ? 'text-brand-600' : 'text-slate-700',
              ]"
            >
              <a> {{ item.name }} </a>
            </t-d>
            <t-d>
              <div class="flex justify-end items-center">
                <FwbToggle color="purple" v-model="toggledPurposes[item.id]" :disabled="facility.archived??false" />
              </div>
            </t-d>
          </t-r>
        </t-body>
        <t-body v-else>
          <t-r>
            <t-d colspan="5" class="text-center">No Purposes to Display</t-d>
          </t-r>
        </t-body>
      </base-table>
    </div>
  </div>
</template>
