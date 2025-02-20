<script setup lang="ts">
import { useNotificationsStore } from '@/stores/data/common/notifications';
import { FwbButton } from 'flowbite-vue';
import { IconX } from '@/components/icons/index';
import ViewJobModal from '../encounters/jobs/partials/modal/ViewJobModal.vue';
import { computed, ref } from 'vue';
import { useEncountersStore } from '@/stores/data/encounters';
import { useJobsStore } from '@/stores/data/encounters/jobs';
import { EncounterStage, Job, User } from '@/api/__generated__/graphql';
import UserAvatar from '@/components/avatar/UserAvatar.vue';
import { useUsersStore } from '@/stores/data/settings/users';

const notificationStore = useNotificationsStore();
const encountersStore = useEncountersStore();
const jobsStore = useJobsStore();
const usersStore = useUsersStore();


const allJobs = computed(() => jobsStore.jobs);

const viewJobModalRef = ref<InstanceType<typeof ViewJobModal>>();

const remove = (id: any) => {
  notificationStore.removeNotification(id);
};

const redirectToEncounter = (id: any) => {
  //TODO: Redirect to EncounterView through Router
  // router.push({ name: 'Home' });

  const encounter = encountersStore.encounters.find(x=> x.id == id);
  
  encountersStore.selectedEncounter = encounter!;
  
  if (encounter?.stage === EncounterStage.Charting) {
    viewJobModalRef.value?.setChartingModalOpen(true);
  } else {
    viewJobModalRef.value?.setModalOpen(true);
  }

  
  notificationStore.removeNotification(id);
};


const getJobAsProp = computed(() => {
  if (!encountersStore.selectedEncounter.jobId) return {} as Job;
  return allJobs.value.find((job) => job.id === encountersStore.selectedEncounter.jobId) as Job;
});
function getUserForAvatar(id: string) {
  return usersStore.users.find((user) => user.id === id) as User;
}
</script>

<template>
  <transition-group name="fade" appear tag="div">
    <div
      v-for="notification in notificationStore.notifications"
      :key="notification.id"
      class="fixed sm:bottom-10 bottom-4 left-1/2 transform -translate-x-1/2 z-50 p-4 rounded-lg sm:w-[365px] w-[320px] bg-slate-800 shadow"
    >
      <div class="flex sm:flex-row flex-col gap-3">
        <div>
          <user-avatar :user="getUserForAvatar(notification.alertedBy)" rounded size="sm" />
        </div>
        <!-- Close button  -->
        <fwb-button
          @click="remove(notification.id)"
          color="light"
          pill
          square
          class="bg-slate-800 border-slate-800 w-8 h-8 flex justify-center items-center hover:border-slate-600 text-slate-500 hover:bg-slate-800 absolute right-[12px] top-3 focus:ring-0"
        >
          <IconX width="20" height="20" />
        </fwb-button>
        <!-- Text -->
        <div class="flex-1">
          <div class="font-bold text-sm text-white mr-8">{{notification.user}} needs help with a Patient.</div>
          <p class="text-sm font-medium text-slate-400 pt-2">
            {{ notification.reason }}
          </p>
          <div class="flex flex-col sm:my-5 my-3">
            <span class="font-bold text-sm text-white">{{ notification.purpose }}</span>
            <p class="text-sm font-medium text-slate-400">{{notification.facility}} / {{ notification.room }}</p>
          </div>
          <div class="flex justify-evenly gap-3">
            <fwb-button
              @click="remove(notification.id)"
              class="flex-1 bg-slate-800 hover:bg-slate-700 border-2 border-slate-600 whitespace-nowrap focus:ring-0"
              pill
              >Not now</fwb-button
            >
            <fwb-button
              @click="redirectToEncounter(notification.id)"
              color="purple"
              class="flex-1 focus:ring-0"
              pill
              >See Details</fwb-button
            >
          </div>
        </div>
      </div>
    </div>
    <!-- View/Open Job Modal -->
    <ViewJobModal
        ref="viewJobModalRef"
        :is-live-queue="true"
        :encounter="encountersStore.selectedEncounter"
        :job="getJobAsProp"
      ></ViewJobModal>
  </transition-group>
</template>

<style scoped>
/* Fade-in Transition */
.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.5s, transform 0.5s ease-out;
}

.fade-enter-from,
.fade-leave-to {
  opacity: 0;
  transform: translateY(20px);
}

.fade-enter-to,
.fade-leave-from {
  opacity: 1;
  transform: translateY(0);
}
</style>