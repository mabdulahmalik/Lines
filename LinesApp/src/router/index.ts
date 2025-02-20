import { createRouter, createWebHistory } from 'vue-router';
import { defineComponent, h } from 'vue';

const LiveQueue = () => import('@/views/encounters/queue/LiveQueue.vue');
const Jobs = () => import('@/views/encounters/jobs/JobsView.vue');
const Lines = () => import('@/views/encounters/lines/LinesView.vue');
const Records = () => import('@/views/encounters/records/RecordsView.vue');

const Facilities = () => import('@/views/facilities/Facilities.vue');

const Reporting = () => import('@/views/reporting/Reporting.vue');

const EncountersSettings = () => import('@/views/settings/encounters/EncountersView.vue');
const FacilitiesSettings = () => import('@/views/settings/facilities/Facilities.vue');
const UsersSetting = () => import('@/views/settings/users/UsersView.vue');

const UserPreferences = () => import('@/views/user/Preferences.vue');
const MyProfile = () => import('@/views/user/Profile.vue');

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'Home',
      redirect: '/encounters/live-queue',
    },
    // Hub
    {
      path: '/hub/mine',
      name: 'Mine',
      component: defineComponent({
        setup() {
          return () => h('h1', 'Mine');
        },
      }),
    },
    {
      path: '/hub/team',
      name: 'Team',
      component: defineComponent({
        setup() {
          return () => h('h1', 'Team');
        },
      }),
    },
    {
      path: '/hub/bussiness',
      name: 'Bussiness',
      component: defineComponent({
        setup() {
          return () => h('h1', 'Bussiness');
        },
      }),
    },
    {
      path: '/hub/org',
      name: 'Organization',
      component: defineComponent({
        setup() {
          return () => h('h1', 'Organization');
        },
      }),
    },

    // Encounters
    {
      path: '/encounters/live-queue',
      name: 'Live Queue',
      component: LiveQueue,
    },
    {
      path: '/encounters/jobs',
      name: 'Jobs',
      component: Jobs,
    },
    {
      path: '/encounters/lines',
      name: 'Lines',
      component: Lines,
    },
    {
      path: '/encounters/records',
      name: 'Records',
      component: Records,
    },
    {
      path: '/reporting',
      name: 'Reporting',
      component: Reporting
    },
    {
      path: '/facilities',
      name: 'Facilities page',
      component:Facilities,
    },
    // settings
    {
      path: '/settings/encounters',
      name: 'Encounters',
      component: EncountersSettings,
    },
    {
      path: '/settings/facilities',
      name: 'Facilities',
      component: FacilitiesSettings,
    },
    {
      path: '/settings/users',
      name: 'Users',
      component: UsersSetting,
    },
    {
      path: '/user/preferences',
      name: 'User Preferences',
      component: UserPreferences,
    },
    // user
    {
      path: '/user/my-profile',
      name: 'My Profile',
      component: MyProfile,
    }
  ],
});

export default router;
