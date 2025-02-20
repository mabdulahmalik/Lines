<script setup lang="ts">
import { FwbButton, FwbToggle } from 'flowbite-vue';
import { IconSettings02 } from '@/components/icons/index';
import { BaseTable, THead, TBody, TR, TD, TH } from '@/components/table/index';
import { computed, ref, watch } from 'vue';
import { useProceduresStore } from '@/stores/data/settings/procedures';
import { Facility } from '@/api/__generated__/graphql';
import { useRouter } from 'vue-router';
import { useSidebarStore } from '@/stores/sidebar';
import { useTabStore } from '@/stores/tab';
const tabStore = useTabStore();

const props = defineProps<{
  facility: Facility;
}>();
const emit = defineEmits(['updateExcludedProcedureIds', 'closeModal']);
const sidebarStore = useSidebarStore();
const router = useRouter();
const proceduresStore = useProceduresStore();
const procedures = computed(() => proceduresStore.procedures);

const activeProcedures = computed(() => {
  return procedures.value.filter((procedure) => procedure.archived === false);
});

// Toggles
const toggledProcedures = ref<{ [key: string]: boolean }>({});

// Initialize the toggled state: all on except those in props.facility.procedureIds (which are off)
procedures.value.forEach((procedure) => {
  toggledProcedures.value[procedure.id] = !(
    props.facility.procedureIds?.includes(procedure.id) ?? false
  );
});

watch(
  toggledProcedures,
  (newVal) => {
    const excludedProcedureIds = Object.keys(newVal).filter((id) => !newVal[id]);
    emit('updateExcludedProcedureIds', excludedProcedureIds);
    // console.log('Excluded Procedure:', excludedProcedureIds);
  },
  { deep: true }
);

// navigate To Settings
const navigateToSettings = () => {
  emit('closeModal', true);
  router.push({ path: '/settings/encounters' });
  sidebarStore.page='settings';
  sidebarStore.selected='Encounters';
  tabStore.setEncountersActiveTab(1);
};
</script>

<template>
  <!-- Procedures -->
  <div>
    <div>
      <div class="font-semibold text-lg">Procedures</div>
      <div class="font-medium text-sm pt-1">
        The Procedures listed below are indicative of the procedures that can be performed at this facility. 
        Toggle the switch to exclude a procedure from the facility and prevent it from being selected during an Encounter.
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
        <t-body v-if="activeProcedures.length > 0">
          <t-r v-for="(item, index) in activeProcedures" :key="index" class="hover:bg-slate-50">
            <t-d
              :class="[
                'font-semibold',
                toggledProcedures[item.id] ? 'text-brand-600' : 'text-slate-700',
              ]"
            >
              <a> {{ item.name }} </a>
            </t-d>
            <t-d>
              <div class="flex justify-end items-center">
                <FwbToggle color="purple" v-model="toggledProcedures[item.id]" :disabled="facility.archived??false" />
              </div>
            </t-d>
          </t-r>
        </t-body>
        <t-body v-else>
          <t-r>
            <t-d colspan="5" class="text-center">No Procedures to Display</t-d>
          </t-r>
        </t-body>
      </base-table>
    </div>
  </div>
</template>
